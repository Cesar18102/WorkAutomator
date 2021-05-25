using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Autofac;

using WorkAutomatorDataAccess;
using WorkAutomatorDataAccess.Entities;
using WorkAutomatorDataAccess.RepoInterfaces;

using WorkAutomatorLogic.Extensions;
using WorkAutomatorLogic.ServiceInterfaces;
using WorkAutomatorLogic.Models.Roles;
using WorkAutomatorLogic.Models.Permission;

namespace WorkAutomatorLogic.Services
{
    internal class InitService : IInitService
    {
        private static IRepo<DbPermissionTypeEntity> DbPermissionTypeRepo = RepoDependencyHolder.ResolveRealRepo<DbPermissionTypeEntity>();
        private static IRepo<DbPermissionEntity> DbPermissionRepo = RepoDependencyHolder.ResolveRealRepo<DbPermissionEntity>();
        private static IRepo<RoleEntity> RoleRepo = RepoDependencyHolder.ResolveRealRepo<RoleEntity>();

        public async Task InitDbPermissions()
        {
            IList<DbPermissionTypeEntity> existingDbPermissionTypes = await DbPermissionTypeRepo.Get();
            IList<DbPermissionEntity> existingDbPermissions = await DbPermissionRepo.Get();

            foreach (string interactionDbTypeName in ModelEntityMapper.INTERACTION_DB_TYPES.Values)
            {
                if (existingDbPermissionTypes.FirstOrDefault(pt => pt.name == interactionDbTypeName) != null)
                    continue;

                await DbPermissionTypeRepo.Create(new DbPermissionTypeEntity() { name = interactionDbTypeName });
            }

            existingDbPermissionTypes = await DbPermissionTypeRepo.Get();
            existingDbPermissions = await DbPermissionRepo.Get();

            foreach (string table in ModelEntityMapper.TABLE_NAME_DICTIONARY.Values)
            {
                DbPermissionEntity[] permissionsForTable = existingDbPermissions.Where(p => p.table_name == table).ToArray();

                foreach (string interactionDbTypeName in ModelEntityMapper.INTERACTION_DB_TYPES.Values)
                {
                    DbPermissionTypeEntity dbPermissionType = existingDbPermissionTypes.First(pt => pt.name == interactionDbTypeName);

                    if (permissionsForTable.FirstOrDefault(p => p.db_permission_type_id == dbPermissionType.id) != null)
                        continue;

                    await DbPermissionRepo.Create(
                        new DbPermissionEntity() { 
                            table_name = table, 
                            db_permission_type_id = dbPermissionType.id 
                        }
                    );
                }
            }
        }

        public async Task InitDefaultRoles()
        {
            IList<RoleEntity> defaultRoles = await RoleRepo.Get(role => role.is_default);
            IList<DbPermissionEntity> permissions = await DbPermissionRepo.Get();

            if (defaultRoles.FirstOrDefault(role => role.name == DefaultRoles.AUTHORIZED.ToName()) == null)
            {
                RoleEntity authorizedRole = new RoleEntity()
                {
                    is_default = true,
                    name = DefaultRoles.AUTHORIZED.ToName()
                };

                authorizedRole.DbPermissions.AddRange(
                    permissions.Where(p => 
                        p.table_name == DbTable.Account.ToName() && (
                            p.DbPermissionType.name == InteractionDbType.READ.ToName() || 
                            p.DbPermissionType.name == InteractionDbType.UPDATE.ToName()
                        )
                    ).ToArray()
                );

                authorizedRole.DbPermissions.AddRange(
                    permissions.Where(p => 
                        p.table_name == DbTable.Company.ToName() &&
                        p.DbPermissionType.name == InteractionDbType.CREATE.ToName()
                    ).ToArray()
                );

                await RoleRepo.Create(authorizedRole);
            }

            if (defaultRoles.FirstOrDefault(role => role.name == DefaultRoles.OWNER.ToName()) == null)
            {
                RoleEntity ownerRole = new RoleEntity()
                {
                    is_default = true,
                    name = DefaultRoles.OWNER.ToName()
                };

                ownerRole.DbPermissions.AddRange(
                    permissions.Where(p => 
                        p.table_name != DbTable.Account.ToName() && 
                        p.table_name != DbTable.DbPermissionType.ToName() && 
                        p.table_name != DbTable.DbPermission.ToName() &&
                        p.table_name != DbTable.DataType.ToName() &&
                        p.table_name != DbTable.VisualizerType.ToName() && (
                            p.table_name != DbTable.Company.ToName() || 
                            p.DbPermissionType.name != InteractionDbType.CREATE.ToName()
                        )
                    ).ToArray()
                );

                await RoleRepo.Create(ownerRole);
            }
        }

        public async Task InitDataTypes()
        {
            
        }

        public async Task InitVisualizerTypes()
        {
            
        }
    }
}
