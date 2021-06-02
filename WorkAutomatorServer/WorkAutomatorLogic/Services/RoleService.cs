using System;
using System.Linq;
using System.Threading.Tasks;

using Constants;
using Dto;

using WorkAutomatorDataAccess;
using WorkAutomatorDataAccess.Entities;

using WorkAutomatorLogic.Aspects;
using WorkAutomatorLogic.Models.Roles;
using WorkAutomatorLogic.ServiceInterfaces;

namespace WorkAutomatorLogic.Services
{
    internal class RoleService : ServiceBase, IRoleService
    {
        [DbPermissionAspect(Action = InteractionDbType.CREATE, Table = DbTable.Role)]
        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.Detector)]
        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.Manufactory)]
        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.PipelineItem)]
        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.StorageCell)]
        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.DbPermission)]
        public async Task<RoleModel> CreateRole(AuthorizedDto<RoleDto> role)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    AccountEntity account = await db.GetRepo<AccountEntity>().Get(role.Session.UserId);

                    RoleEntity roleEntity = new RoleEntity();

                    roleEntity.name = role.Data.Name;
                    roleEntity.company_id = role.Data.CompanyId.Value;

                    roleEntity.ManufactoryPermissions = account.Roles.SelectMany(r => r.ManufactoryPermissions).Where(
                        manufactory => role.Data.ManufactoryIds.Contains(manufactory.id)
                    ).ToList();

                    roleEntity.PipelineItemPermissions = account.Roles.SelectMany(r => r.PipelineItemPermissions).Where(
                        pi => role.Data.PipelineItemIds.Contains(pi.id)
                    ).ToList();

                    roleEntity.StorageCellPermissions = account.Roles.SelectMany(r => r.StorageCellPermissions).Where(
                        storageCell => role.Data.StorageCellIds.Contains(storageCell.id)
                    ).ToList();

                    roleEntity.DetectorPermissions = account.Roles.SelectMany(r => r.DetectorPermissions).Where(
                        detector => role.Data.DetectorIds.Contains(detector.id)
                    ).ToList();

                    roleEntity.DbPermissions = account.Roles.SelectMany(r => r.DbPermissions).Where(
                        dbPermission => role.Data.DbPermissionIds.Contains(dbPermission.id)
                    ).ToList();

                    await db.GetRepo<RoleEntity>().Create(roleEntity);
                    await db.Save();

                    return roleEntity.ToModel<RoleModel>();
                }
            });
        }

        public async Task<RoleModel> UpdateRole(AuthorizedDto<RoleDto> role)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    AccountEntity account = await db.GetRepo<AccountEntity>().Get(role.Session.UserId);

                    RoleEntity roleEntity = new RoleEntity();
                    roleEntity.name = role.Data.Name;

                    ManufactoryEntity[] manufactoryPermissions = account.Roles.SelectMany(r => r.ManufactoryPermissions).Where(
                        manufactory => role.Data.ManufactoryIds.Contains(manufactory.id)
                    ).ToArray();

                    foreach (ManufactoryEntity manufactoryToRemove in roleEntity.ManufactoryPermissions.Except(manufactoryPermissions).ToArray())
                        roleEntity.ManufactoryPermissions.Remove(manufactoryToRemove);

                    foreach(ManufactoryEntity manufactoryToAdd in manufactoryPermissions.Except(roleEntity.ManufactoryPermissions).ToArray())
                        roleEntity.ManufactoryPermissions.Add(manufactoryToAdd);


                    PipelineItemEntity[] pipelineItemPermissions = account.Roles.SelectMany(r => r.PipelineItemPermissions).Where(
                        pipelineItem => role.Data.PipelineItemIds.Contains(pipelineItem.id)
                    ).ToArray();

                    foreach (PipelineItemEntity pipelineItemToRemove in roleEntity.PipelineItemPermissions.Except(pipelineItemPermissions).ToArray())
                        roleEntity.PipelineItemPermissions.Remove(pipelineItemToRemove);

                    foreach (PipelineItemEntity pipelineItemToAdd in pipelineItemPermissions.Except(roleEntity.PipelineItemPermissions).ToArray())
                        roleEntity.PipelineItemPermissions.Add(pipelineItemToAdd);


                    StorageCellEntity[] storageCellPermissions = account.Roles.SelectMany(r => r.StorageCellPermissions).Where(
                        storageCell => role.Data.StorageCellIds.Contains(storageCell.id)
                    ).ToArray();

                    foreach (StorageCellEntity storageCellToRemove in roleEntity.StorageCellPermissions.Except(storageCellPermissions).ToArray())
                        roleEntity.StorageCellPermissions.Remove(storageCellToRemove);

                    foreach (StorageCellEntity storageCellToAdd in storageCellPermissions.Except(roleEntity.StorageCellPermissions).ToArray())
                        roleEntity.StorageCellPermissions.Add(storageCellToAdd);


                    DetectorEntity[] detectorPermissions = account.Roles.SelectMany(r => r.DetectorPermissions).Where(
                        detector => role.Data.DetectorIds.Contains(detector.id)
                    ).ToArray();

                    foreach (DetectorEntity detectorToRemove in roleEntity.DetectorPermissions.Except(detectorPermissions).ToArray())
                        roleEntity.DetectorPermissions.Remove(detectorToRemove);

                    foreach (DetectorEntity detectorToAdd in detectorPermissions.Except(roleEntity.DetectorPermissions).ToArray())
                        roleEntity.DetectorPermissions.Add(detectorToAdd);


                    DbPermissionEntity[] dbPermissions = account.Roles.SelectMany(r => r.DbPermissions).Where(
                        dbPermission => role.Data.DbPermissionIds.Contains(dbPermission.id)
                    ).ToArray();

                    foreach (DbPermissionEntity dbPermissionToRemove in roleEntity.DbPermissions.Except(dbPermissions).ToArray())
                        roleEntity.DbPermissions.Remove(dbPermissionToRemove);

                    foreach (DbPermissionEntity dbPermissionToAdd in dbPermissions.Except(roleEntity.DbPermissions).ToArray())
                        roleEntity.DbPermissions.Add(dbPermissionToAdd);

                    await db.Save();

                    return roleEntity.ToModel<RoleModel>();
                }
            });
        }

        public Task GrantRole(AuthorizedDto<GrantUngrantRoleDto> role)
        {
            throw new NotImplementedException();
        }

        public Task UnGrantRole(AuthorizedDto<GrantUngrantRoleDto> role)
        {
            throw new NotImplementedException();
        }
    }
}
