using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations.Schema;

using Constants;

using WorkAutomatorDataAccess;
using WorkAutomatorDataAccess.Entities;

using WorkAutomatorLogic.Exceptions;
using WorkAutomatorLogic.Extensions;
using WorkAutomatorLogic.Models.Permission;
using WorkAutomatorLogic.ServiceInterfaces;

namespace WorkAutomatorLogic.Services
{
    internal class PermissionService : IPermissionService
    {


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

                if (interaction.InteractionType == InteractionType.DB)
                {
                    string tableName = ModelEntityMapper.TABLE_NAME_DICTIONARY[interaction.Table];

                    DbPermissionEntity[] dbPermissions = initiator.Roles.SelectMany(role => role.DbPermissions).Where(
                        dbPermission => tableName == dbPermission.table_name
                    ).ToArray();

                    string[] requiredInteractionTypes = interaction.InteractionDbType.GetFlags().Select(
                        flag => ModelEntityMapper.INTERACTION_DB_TYPES[flag]
                    ).ToArray();

                    string[] havingInteractionTypes = dbPermissions.Select(
                        dbPermission => dbPermission.DbPermissionType.name
                    ).ToArray();

                    bool isLegal = requiredInteractionTypes.Intersect(havingInteractionTypes).Count() == requiredInteractionTypes.Length;

                    if (interaction.CompanyId.HasValue)
                    {
                        CompanyEntity company = await db.GetRepo<CompanyEntity>().FirstOrDefault(
                            c => c.owner_id == interaction.CompanyId.Value
                        );

                        if(company == null)
                            throw new NotFoundException("Company");
                    }
                            
                    if (isLegal)
                    {
                        if (interaction.CompanyId.HasValue)
                            isLegal &= interaction.CompanyId == initiator.company_id;
                        else
                            interaction.CompanyId = initiator.company_id;
                    }

                    if(isLegal && interaction.ObjectIds != null && interaction.ObjectIds.Length != 0)
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

                        IEnumerable<Task> tasks = interaction.ObjectIds.Select(
                            subjectId => getByIdMethod.Invoke(repo, new object[] { subjectId })
                        ).Cast<Task>();

                        object[] entities = tasks.Select(task =>
                        {
                            object awaiter = typeof(Task<>)
                                .MakeGenericType(entityType)
                                .GetMethod(nameof(Task<object>.GetAwaiter))
                                .Invoke(task, new object[] { });

                            object entity = typeof(TaskAwaiter<>)
                                .MakeGenericType(entityType)
                                .GetMethod(nameof(TaskAwaiter<object>.GetResult))
                                .Invoke(awaiter, new object[] { });

                            if (entity == null)
                            {
                                throw new NotFoundException(
                                    entityType.Name.Replace("Entity", "")
                                );
                            }

                            return entity;
                        }).ToArray();

                        if (interaction.CheckSameCompany && interaction.CompanyId.HasValue)
                        {
                            foreach (object entity in entities)
                            {
                                isLegal &= (bool)entity.GetType().GetMethod(
                                    nameof(EntityBase.IsOwnedByCompany)
                                ).Invoke(
                                    entity, 
                                    new object[] {
                                        interaction.CompanyId.Value
                                    }
                                );

                                if (!isLegal)
                                    break;
                            }
                        }
                    }

                    if (!isLegal)
                    {
                        if (interaction.CompanyId.HasValue && interaction.CheckSameCompany)
                        {
                            if (interaction.ObjectIds != null && interaction.ObjectIds.Length != 0)
                            {
                                notEnoughPermissions = requiredInteractionTypes.Select(
                                    t => $"{t} {interaction.Table} {string.Join(", ", interaction.ObjectIds.Select(id => "#" + id))}"
                                ).ToArray();
                            }
                            else
                            {
                                notEnoughPermissions = requiredInteractionTypes.Select(
                                    t => $"{t} {interaction.Table} for company #{interaction.CompanyId.Value}"
                                ).ToArray();
                            }
                        }
                        else
                        {
                            notEnoughPermissions = requiredInteractionTypes.Except(havingInteractionTypes).Select(
                                t => $"{t} {interaction.Table}"
                            ).ToArray();
                        }
                    }
                }
                else
                {
                    Func<RoleEntity, IEnumerable<IdEntity>> permittedTargetCollectionSelector = null;

                    switch (interaction.InteractionType)
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

                    int[] permittedIds = initiator.Roles.SelectMany(permittedTargetCollectionSelector)
                        .Select(target => target.id).ToArray();

                    int[] notProvidedPermissions = interaction.ObjectIds.Except(permittedIds).ToArray();

                    if (notProvidedPermissions.Length != 0)
                    {
                        notEnoughPermissions = notProvidedPermissions.Select(
                            permission => $"Permission to interact with {interaction.InteractionType} #{permission}"
                        ).ToArray();
                    }
                }

                if (notEnoughPermissions != null)
                    throw new NotPermittedException(notEnoughPermissions);
            }
        }

        public async Task<PermissionDbModel[]> GetDbPermissions()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                IList<DbPermissionEntity> dbPermissions = await db.GetRepo<DbPermissionEntity>().Get();
                return ModelEntityMapper.Mapper.Map<IList<PermissionDbModel>>(dbPermissions).ToArray();
            }
        }
    }
}