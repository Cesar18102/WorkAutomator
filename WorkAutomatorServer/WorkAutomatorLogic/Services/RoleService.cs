using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Constants;
using Dto;

using WorkAutomatorDataAccess;
using WorkAutomatorDataAccess.Entities;

using WorkAutomatorLogic.Aspects;
using WorkAutomatorLogic.Exceptions;
using WorkAutomatorLogic.Models;
using WorkAutomatorLogic.Models.Roles;
using WorkAutomatorLogic.ServiceInterfaces;

namespace WorkAutomatorLogic.Services
{
    internal class RoleService : ServiceBase, IRoleService
    {
        [DbPermissionAspect(Action = InteractionDbType.CREATE, Table = DbTable.Role)]
        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.Detector, CheckSameCompany = true)]
        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.Manufactory, CheckSameCompany = true)]
        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.PipelineItem, CheckSameCompany = true)]
        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.StorageCell, CheckSameCompany = true)]
        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.DbPermission, CheckSameCompany = true)]
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

        [DbPermissionAspect(Action = InteractionDbType.UPDATE, Table = DbTable.Role, CheckSameCompany = true)]
        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.Detector, CheckSameCompany = true)]
        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.Manufactory, CheckSameCompany = true)]
        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.PipelineItem, CheckSameCompany = true)]
        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.StorageCell, CheckSameCompany = true)]
        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.DbPermission, CheckSameCompany = true)]
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

        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.Role, CheckSameCompany = true)]
        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.Account, CheckSameCompany = true)]
        public async Task<WorkerModel> GrantRole(AuthorizedDto<GrantUngrantRoleDto> role)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    IRepo<AccountEntity> accountRepo = db.GetRepo<AccountEntity>();

                    AccountEntity initiator = await accountRepo.Get(role.Session.UserId);
                    AccountEntity grantedTo = await accountRepo.Get(role.Data.GrantToAccountId.Value);

                    RoleEntity roleEntity = await db.GetRepo<RoleEntity>().Get(role.Data.RoleId.Value);

                    if (!grantedTo.Bosses.Contains(initiator))
                        throw new NotPermittedException($"Boss for Account #{grantedTo}");

                    if(!IsRolesPermissionsIncludeRolePermissions(initiator.Roles.ToArray(), roleEntity))
                        throw new NotPermittedException($"Granted permissions must be presented for Initiator");

                    grantedTo.Roles.Add(roleEntity);

                    await db.Save();

                    return grantedTo.ToModel<WorkerModel>();
                }
            });
        }

        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.Role, CheckSameCompany = true)]
        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.Account, CheckSameCompany = true)]
        public async Task<WorkerModel> UnGrantRole(AuthorizedDto<GrantUngrantRoleDto> role)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    IRepo<AccountEntity> accountRepo = db.GetRepo<AccountEntity>();

                    AccountEntity initiator = await accountRepo.Get(role.Session.UserId);
                    AccountEntity grantedTo = await accountRepo.Get(role.Data.GrantToAccountId.Value);

                    RoleEntity roleEntity = await db.GetRepo<RoleEntity>().Get(role.Data.RoleId.Value);

                    if (!grantedTo.Bosses.Contains(initiator))
                        throw new NotPermittedException($"Boss for Account #{grantedTo}");

                    if (!IsRolesPermissionsIncludeRolePermissions(initiator.Roles.ToArray(), roleEntity))
                        throw new NotPermittedException($"Granted permissions must be presented for Initiator");

                    grantedTo.Roles.Remove(roleEntity);

                    await db.Save();

                    return grantedTo.ToModel<WorkerModel>();
                }
            });
        }

        public async Task CreateCompanyOwnerRole(int companyId)
        {
            await Execute(async () =>
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    CompanyEntity company = await db.GetRepo<CompanyEntity>().Get(companyId);

                    RoleEntity ownerRole = new RoleEntity()
                    {
                        company_id = company.owner_id,
                        is_default = true,
                        name = $"COMPANY #{company.owner_id} OWNER",
                        ManufactoryPermissions = company.Manufactories,
                        PipelineItemPermissions = company.PipelineItemPrefabs.SelectMany(pi => pi.pipeline_item).ToList(),
                        StorageCellPermissions = company.StorageCellPrefabs.SelectMany(sc => sc.storage_cell).ToList(),
                        DetectorPermissions = company.DetectorPrefabs.SelectMany(d => d.detector).ToList()
                    };

                    await db.GetRepo<RoleEntity>().Create(ownerRole);

                    company.Owner.Roles.Add(ownerRole);

                    await db.Save();
                }
            });
        }

        public async Task<RoleEntity> GetCompanyOwnerRole(int companyId, UnitOfWork db)
        {
            CompanyEntity company = await db.GetRepo<CompanyEntity>().Get(companyId);
            return company.Owner.Roles.FirstOrDefault(role => role.is_default && role.name == $"COMPANY #{companyId} OWNER");
        }

        public async Task UpdateCompanyOwnerRole(int companyId)
        {
            await Execute(async () =>
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    CompanyEntity company = await db.GetRepo<CompanyEntity>().Get(companyId);
                    RoleEntity ownerRole = await GetCompanyOwnerRole(companyId, db);

                    foreach (ManufactoryEntity manufactoryToRemove in ownerRole.ManufactoryPermissions.Except(company.Manufactories).ToArray())
                        ownerRole.ManufactoryPermissions.Remove(manufactoryToRemove);
                    foreach (ManufactoryEntity manufactoryToAdd in company.Manufactories.Except(ownerRole.ManufactoryPermissions).ToArray())
                        ownerRole.ManufactoryPermissions.Add(manufactoryToAdd);

                    PipelineItemEntity[] allPipelineItems = company.PipelineItemPrefabs.SelectMany(pip => pip.pipeline_item).ToArray();
                    foreach (PipelineItemEntity pipelineItemToRemove in ownerRole.PipelineItemPermissions.Except(allPipelineItems).ToArray())
                        ownerRole.PipelineItemPermissions.Remove(pipelineItemToRemove);
                    foreach (PipelineItemEntity pipelineItemToAdd in allPipelineItems.Except(ownerRole.PipelineItemPermissions).ToArray())
                        ownerRole.PipelineItemPermissions.Add(pipelineItemToAdd);


                    StorageCellEntity[] allStorageCells = company.StorageCellPrefabs.SelectMany(sc => sc.storage_cell).ToArray();
                    foreach (StorageCellEntity storageCellToRemove in ownerRole.StorageCellPermissions.Except(allStorageCells).ToArray())
                        ownerRole.StorageCellPermissions.Remove(storageCellToRemove);
                    foreach (StorageCellEntity storageCellToAdd in allStorageCells.Except(ownerRole.StorageCellPermissions).ToArray())
                        ownerRole.StorageCellPermissions.Add(storageCellToAdd);


                    DetectorEntity[] allDetectors = company.DetectorPrefabs.SelectMany(dp => dp.detector).ToArray();
                    foreach (DetectorEntity detectorToRemove in ownerRole.DetectorPermissions.Except(allDetectors).ToArray())
                        ownerRole.DetectorPermissions.Remove(detectorToRemove);

                    foreach (DetectorEntity detectorToAdd in allDetectors.Except(ownerRole.DetectorPermissions).ToArray())
                        ownerRole.DetectorPermissions.Add(detectorToAdd);

                    await db.Save();
                }
            });
        }

        public async Task CreateCompanyWorkerRole(int accountId)
        {
            await Execute(async () =>
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    AccountEntity account = await db.GetRepo<AccountEntity>().Get(accountId);

                    RoleEntity workerRole = new RoleEntity()
                    {
                        company_id = account.company_id,
                        name = $"COMPANY #{account.company_id} MEMBER #{accountId}",
                        is_default = true
                    };

                    await db.GetRepo<RoleEntity>().Create(workerRole);

                    account.Roles.Add(workerRole);

                    await db.Save();
                }
            });
        }

        public async Task<RoleEntity> GetCompanyWorkerRole(int accountId, UnitOfWork db)
        {
            AccountEntity account = await db.GetRepo<AccountEntity>().Get(accountId);
            return account.Roles.FirstOrDefault(role => role.is_default && role.name == $"COMPANY #{account.company_id} MEMBER #{accountId}");
        }

        private bool IsRolesPermissionsIncludeRolePermissions(RoleEntity[] roles, RoleEntity testedRole)
        {
            return roles.SelectMany(r => r.DbPermissions).Intersect(testedRole.DbPermissions).Count() == testedRole.DbPermissions.Count &&
                   roles.SelectMany(r => r.ManufactoryPermissions).Intersect(testedRole.ManufactoryPermissions).Count() == testedRole.ManufactoryPermissions.Count &&
                   roles.SelectMany(r => r.PipelineItemPermissions).Intersect(testedRole.PipelineItemPermissions).Count() == testedRole.PipelineItemPermissions.Count &&
                   roles.SelectMany(r => r.StorageCellPermissions).Intersect(testedRole.StorageCellPermissions).Count() == testedRole.StorageCellPermissions.Count &&
                   roles.SelectMany(r => r.DetectorPermissions).Intersect(testedRole.DetectorPermissions).Count() == testedRole.DetectorPermissions.Count;
        }
    }
}
