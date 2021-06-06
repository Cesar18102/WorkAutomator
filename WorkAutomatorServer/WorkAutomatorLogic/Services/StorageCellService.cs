using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Autofac;

using Constants;

using Dto;
using Dto.Pipeline;
using Dto.Interaction;

using WorkAutomatorDataAccess;
using WorkAutomatorDataAccess.Entities;

using WorkAutomatorLogic.Aspects;
using WorkAutomatorLogic.Models.Pipeline;
using WorkAutomatorLogic.ServiceInterfaces;
using WorkAutomatorLogic.Exceptions;

namespace WorkAutomatorLogic.Services
{
    internal class StorageCellService : ServiceBase, IStorageCellService
    {
        private static RoleService RoleService = LogicDependencyHolder.Dependencies.Resolve<RoleService>();

        [DbPermissionAspect(Action = InteractionDbType.CREATE, Table = DbTable.StorageCell)]
        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.StorageCellPrefab, CheckSameCompany = true)]
        public async Task<StorageCellModel> Create(AuthorizedDto<StorageCellDto> dto)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    StorageCellEntity storageCellEntity = dto.Data.ToModel<StorageCellModel>().ToEntity<StorageCellEntity>();

                    await db.GetRepo<StorageCellEntity>().Create(storageCellEntity);
                    await db.Save();

                    StorageCellPrefabEntity prefab = await db.GetRepo<StorageCellPrefabEntity>().Get(storageCellEntity.storage_cell_prefab_id);

                    RoleEntity ownerRole = await RoleService.GetCompanyOwnerRole(prefab.company_id, db);
                    ownerRole.StorageCellPermissions.Add(storageCellEntity);

                    RoleEntity creatorRole = await RoleService.GetCompanyWorkerRole(dto.Session.UserId, db);
                    creatorRole?.StorageCellPermissions.Add(storageCellEntity);

                    await db.Save();

                    return storageCellEntity.ToModel<StorageCellModel>();
                }
            });
        }

        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.StorageCell, CheckSameCompany = true)]
        public async Task<ICollection<StorageCellModel>> Get(AuthorizedDto<CompanyDto> dto)
        {
            return await Execute(async () =>
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    IList<StorageCellEntity> storageCells = await db.GetRepo<StorageCellEntity>().Get(
                        s => s.StorageCellPrefab.company_id == dto.Data.CompanyId.Value
                    );

                    return ModelEntityMapper.Mapper.Map<IList<StorageCellModel>>(storageCells);
                }
            });
        }

        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.StorageCell, CheckSameCompany = true)]
        public async Task TryInteract(StorageCellInteractionDto dto)
        {
            await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    StorageCellEntity storageCell = await db.GetRepo<StorageCellEntity>().Get(dto.StorageCellId.Value);
                    AccountEntity account = await db.GetRepo<AccountEntity>().Get(dto.AccountId.Value);

                    StorageCellEventEntity storageCellEvent = new StorageCellEventEntity()
                    {
                        account_id = account.id,
                        storage_cell_id = storageCell.id,
                        timespan = DateTime.Now
                    };

                    NotPermittedException ex = null;

                    if (account.Roles.SelectMany(r => r.StorageCellPermissions).Any(m => m.id == storageCell.id))
                        storageCellEvent.log = $"Interaction with Storage cell #{storageCell.id} by Account #{account.id}: SUCCESS";
                    else
                    {
                        storageCellEvent.log = $"Interaction with Storage cell #{storageCell.id} by Account #{account.id}: ACCESS DENIED";
                        ex = new NotPermittedException(storageCellEvent.log);
                    }

                    await db.GetRepo<StorageCellEventEntity>().Create(storageCellEvent);
                    await db.Save();

                    if (ex != null)
                        throw ex;
                }
            });
        }
    }
}
