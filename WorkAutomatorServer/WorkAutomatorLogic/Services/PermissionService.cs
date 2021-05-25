using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations.Schema;

using Attributes;

using WorkAutomatorDataAccess;
using WorkAutomatorDataAccess.Entities;

using WorkAutomatorLogic.Aspects;
using WorkAutomatorLogic.Exceptions;
using WorkAutomatorLogic.Extensions;
using WorkAutomatorLogic.Models.Permission;
using WorkAutomatorLogic.ServiceInterfaces;

namespace WorkAutomatorLogic.Services
{
    internal class PermissionService : IPermissionService
    {
        [DbPermissionAspect(Table = DbTable.DbPermission, Action = InteractionDbType.CREATE)]
        public Task CreateDbPermission(PermissionDbModel dbPermission, [InitiatorAccountId] int creatorAccountId)
        {
            throw new System.NotImplementedException();
        }

        [DbPermissionAspect(DbTableConverterType = typeof(PermissionModelBaseToTableNameConverter), Action = InteractionDbType.CREATE)]
        public Task GrantPermission([TableNameParameter] PermissionModelBase permission, [InitiatorAccountId] int grantingByAccountId, int grantingToRoleId)
        {
            throw new System.NotImplementedException();
        }

        [DbPermissionAspect(DbTableConverterType = typeof(PermissionModelBaseToTableNameConverter), Action = InteractionDbType.DELETE)]
        public Task UnGrantPermission([TableNameParameter] PermissionModelBase permission, [InitiatorAccountId] int grantingByAccountId, int grantingToRoleId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> IsLegal(Interaction interaction)
        {
            try { await CheckLegal(interaction); return true; }
            catch(NotPermittedException) { return false; }
        }

        public async Task CheckLegal(Interaction interaction)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                AccountEntity initiator = await db.GetRepo<AccountEntity>().Get(interaction.InitiatorAccountId);
                string[] notEnoughPermissions = null;

                if (interaction.Permission is PermissionDbModel dbPermissionModel)
                {
                    string tableName = ModelEntityMapper.TABLE_NAME_DICTIONARY[dbPermissionModel.DbTable];

                    DbPermissionEntity[] dbPermissions = initiator.Roles.SelectMany(role => role.DbPermissions).Where(
                        dbPermission => tableName == dbPermission.table_name
                    ).ToArray();

                    string[] requiredInteractionTypes = dbPermissionModel.InteractionDbType.GetFlags().Select(
                        flag => ModelEntityMapper.INTERACTION_DB_TYPES[flag]
                    ).ToArray();

                    string[] havingInteractionTypes = dbPermissions.Select(
                        dbPermission => dbPermission.DbPermissionType.name
                    ).ToArray();

                    bool isLegal = requiredInteractionTypes.Intersect(havingInteractionTypes).Count() == requiredInteractionTypes.Length;

                    if(interaction.SubjectId.HasValue)
                    {
                        Type entityType = typeof(EntityBase).Assembly.GetTypes().FirstOrDefault(
                            type => type.GetCustomAttribute<TableAttribute>()?.Name == tableName
                        );

                        object repo = db.GetType()
                            .GetMethod(nameof(UnitOfWork.GetRepo))
                            .MakeGenericMethod(entityType)
                            .Invoke(db, new object[] { });

                        MethodInfo getByIdMethod = repo.GetType().GetMethods().FirstOrDefault(
                            method =>
                            {
                                if (method.Name != nameof(IRepo<EntityBase>.Get))
                                    return false;

                                ParameterInfo[] parameters = method.GetParameters();
                                return parameters.Count() == 1 && parameters[0].Name == "id";
                            }
                        );

                        object task = getByIdMethod.Invoke(repo, new object[] { interaction.SubjectId.Value });

                        object awaiter = typeof(Task<>)
                            .MakeGenericType(entityType)
                            .GetMethod(nameof(Task<object>.GetAwaiter))
                            .Invoke(task, new object[] { });

                        object entity = typeof(TaskAwaiter<>)
                            .MakeGenericType(entityType)
                            .GetMethod(nameof(TaskAwaiter<object>.GetResult))
                            .Invoke(awaiter, new object[] { });

                        if (entity == null)
                            throw new NotFoundException(entityType.Name.Replace("Entity", ""));

                        if (entity is CompanyEntity)
                            isLegal &= initiator.Company?.owner_id == (entity as CompanyEntity).owner_id;
                        else
                        {
                            int companyId = (int)entity.GetType().GetProperty("company_id").GetValue(entity);
                            isLegal &= companyId == initiator.Company?.owner_id;
                        }
                    }

                    if (!isLegal)
                    {
                        if (interaction.SubjectId.HasValue)
                        {
                            notEnoughPermissions = requiredInteractionTypes.Select(
                                t => $"{t} {dbPermissionModel.DbTable} #{interaction.SubjectId.Value}"
                            ).ToArray();
                        }
                        else
                        {
                            notEnoughPermissions = requiredInteractionTypes.Except(havingInteractionTypes).Select(
                                t => $"{t} {dbPermissionModel.DbTable}"
                            ).ToArray();
                        }
                    }
                }
                else if (interaction.Permission is PermissionModel commonPermission)
                {
                    Func<RoleEntity, IEnumerable<IdEntity>> permittedTargetCollectionSelector = null;

                    switch (commonPermission.InteractionType)
                    {
                        case InteractionType.DETECTOR:
                            permittedTargetCollectionSelector = role => role.DetectorPermissions;
                            break;

                        case InteractionType.MANUFACTORY:
                            permittedTargetCollectionSelector = role => role.ManufactoryPermissions;
                            break;

                        case InteractionType.PIPELINE_ITEM:
                            permittedTargetCollectionSelector = role => role.PipelineItemPermissions;
                            break;

                        case InteractionType.STORAGE:
                            permittedTargetCollectionSelector = role => role.StorageCellPermissions;
                            break;
                    }

                    bool isLegal = initiator.Roles.SelectMany(permittedTargetCollectionSelector).FirstOrDefault(
                        target => target.id == commonPermission.InteractionTargetId
                    ) == null;

                    if (!isLegal)
                    {
                        notEnoughPermissions = new string[1] {
                            $"Permission to interact with {commonPermission.InteractionType} #{commonPermission.InteractionTargetId}"
                        };
                    }
                }

                if (notEnoughPermissions != null)
                    throw new NotPermittedException(notEnoughPermissions);
            }
        }
    }
}