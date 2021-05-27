using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Autofac;

using Constants;

using WorkAutomatorLogic.Extensions;
using WorkAutomatorLogic.ServiceInterfaces;

using WorkAutomatorDataAccess;
using WorkAutomatorDataAccess.Entities;

namespace WorkAutomatorLogic.Services
{
    internal class InitService : ServiceBase, IInitService
    {
        public async Task InitDbPermissions()
        {
            await Execute(async () =>
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    IRepo<DbPermissionTypeEntity> dbPermissionTypeRepo = db.GetRepo<DbPermissionTypeEntity>();

                    IList<DbPermissionTypeEntity> existingDbPermissionTypes = await dbPermissionTypeRepo.Get();
                    IList<DbPermissionEntity> existingDbPermissions = await db.GetRepo<DbPermissionEntity>().Get();

                    foreach (string interactionDbTypeName in ModelEntityMapper.INTERACTION_DB_TYPES.Values)
                    {
                        if (existingDbPermissionTypes.FirstOrDefault(pt => pt.name == interactionDbTypeName) != null)
                            continue;

                        await dbPermissionTypeRepo.Create(new DbPermissionTypeEntity() { name = interactionDbTypeName });
                    }

                    await db.Save();
                }

                using (UnitOfWork db = new UnitOfWork())
                {
                    IRepo<DbPermissionEntity> dbPermissionRepo = db.GetRepo<DbPermissionEntity>();

                    IList<DbPermissionTypeEntity> existingDbPermissionTypes = await db.GetRepo<DbPermissionTypeEntity>().Get();
                    IList<DbPermissionEntity> existingDbPermissions = await dbPermissionRepo.Get();

                    foreach (string table in ModelEntityMapper.TABLE_NAME_DICTIONARY.Values)
                    {
                        DbPermissionEntity[] permissionsForTable = existingDbPermissions.Where(p => p.table_name == table).ToArray();

                        foreach (string interactionDbTypeName in ModelEntityMapper.INTERACTION_DB_TYPES.Values)
                        {
                            DbPermissionTypeEntity dbPermissionType = existingDbPermissionTypes.First(pt => pt.name == interactionDbTypeName);

                            if (permissionsForTable.FirstOrDefault(p => p.db_permission_type_id == dbPermissionType.id) != null)
                                continue;

                            await dbPermissionRepo.Create(
                                new DbPermissionEntity()
                                {
                                    table_name = table,
                                    db_permission_type_id = dbPermissionType.id
                                }
                            );
                        }
                    }

                    await db.Save();
                }
            });
        }

        public async Task InitDefaultRoles()
        {
            await Execute(async () =>
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    IRepo<RoleEntity> roleRepo = db.GetRepo<RoleEntity>();

                    IList<RoleEntity> defaultRoles = await roleRepo.Get(role => role.is_default);
                    IList<DbPermissionEntity> permissions = await db.GetRepo<DbPermissionEntity>().Get();

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

                        await roleRepo.Create(authorizedRole);
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
                                p.table_name != DbTable.DbPermissionType.ToName() &&
                                p.table_name != DbTable.DbPermission.ToName() &&
                                p.table_name != DbTable.DataType.ToName() &&
                                p.table_name != DbTable.VisualizerType.ToName() && (
                                    p.table_name != DbTable.Company.ToName() ||
                                    p.DbPermissionType.name != InteractionDbType.CREATE.ToName()
                                )
                            ).ToArray()
                        );

                        await roleRepo.Create(ownerRole);
                    }

                    await db.Save();
                }
            });
        }

        public async Task InitDataTypes()
        {
            
        }

        public async Task InitVisualizerTypes()
        {
            
        }
    }
}
