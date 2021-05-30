using System.Collections.Generic;
using System.Threading.Tasks;

using Constants;

using Dto;
using Dto.Pipeline;

using WorkAutomatorDataAccess;
using WorkAutomatorDataAccess.Entities;

using WorkAutomatorLogic.Aspects;
using WorkAutomatorLogic.Models.Pipeline;
using WorkAutomatorLogic.ServiceInterfaces;

namespace WorkAutomatorLogic.Services
{
    internal class StorageCellService : ServiceBase, IStorageCellService
    {
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
    }
}
