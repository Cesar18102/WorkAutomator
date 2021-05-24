using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WorkAutomatorDataAccess;
using WorkAutomatorDataAccess.Entities;
using WorkAutomatorDataAccess.RepoInterfaces;

using WorkAutomatorLogic.Aspects;
using WorkAutomatorLogic.Exceptions;
using WorkAutomatorLogic.Extensions;
using WorkAutomatorLogic.Models.Permission;
using WorkAutomatorLogic.ServiceInterfaces;

namespace WorkAutomatorLogic.Services
{
    internal class PermissionService : IPermissionService
    {
        private static IRepo<AccountEntity> AccountRepo = RepoDependencyHolder.ResolveRealRepo<AccountEntity>();
        private static IRepo<DbPermissionEntity> DbPermissionRepo = RepoDependencyHolder.ResolveRealRepo<DbPermissionEntity>();

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
            string[] notEnoughPermissions = null;
            AccountEntity initiator = await AccountRepo.Get(interaction.InitiatorAccountId);

            if (interaction.Permission is PermissionDbModel dbPermissionModel)
            {
                DbPermissionEntity[] dbPermissions = initiator.Roles.SelectMany(role => role.DbPermissions).Where(
                    dbPermission => ModelEntityMapper.TABLE_NAME_DICTIONARY[dbPermissionModel.DbTable] == dbPermission.table_name
                ).ToArray();

                string[] requiredInteractionTypes = dbPermissionModel.InteractionDbType.GetFlags().Select(
                    flag => ModelEntityMapper.INTERACTION_DB_TYPES[flag]
                ).ToArray();

                string[] havingInteractionTypes = dbPermissions.Select(
                    dbPermission => dbPermission.DbPermissionType.name
                ).ToArray();

                bool isLegal = requiredInteractionTypes.Intersect(havingInteractionTypes).Count() == requiredInteractionTypes.Length;

                if (!isLegal)
                    notEnoughPermissions = requiredInteractionTypes.Except(havingInteractionTypes).Select(t => $"{t} {dbPermissionModel.DbTable}").ToArray();
            }
            else if (interaction.Permission is PermissionModel commonPermission)
            {
                Func<RoleEntity, IEnumerable<EntityBase>> permittedTargetCollectionSelector = null;

                switch(commonPermission.InteractionType)
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