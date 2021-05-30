using System.Linq;
using System.Threading.Tasks;

using Constants;

using Dto;
using Dto.Prefabs;

using WorkAutomatorDataAccess;
using WorkAutomatorDataAccess.Entities;

using WorkAutomatorLogic.Aspects;
using WorkAutomatorLogic.Models.Prefabs;
using WorkAutomatorLogic.ServiceInterfaces;

namespace WorkAutomatorLogic.Services
{
    internal class PrefabService : ServiceBase, IPrefabService
    {
        [DbPermissionAspect(Action = InteractionDbType.CREATE, Table = DbTable.PipelineItemPrefab)]
        [DbPermissionAspect(Action = InteractionDbType.CREATE, Table = DbTable.PipelineItemSettingsPrefab)]
        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.DataType)]
        public async Task<PipelineItemPrefabModel> CreatePipelineItemPrefab(AuthorizedDto<PipelineItemPrefabDto> dto)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    PipelineItemPrefabEntity pipelineItemPrefabEntity = dto.Data.ToModel<PipelineItemPrefabModel>().ToEntity<PipelineItemPrefabEntity>();

                    await db.GetRepo<PipelineItemPrefabEntity>().Create(pipelineItemPrefabEntity);
                    await db.Save();

                    return pipelineItemPrefabEntity.ToModel<PipelineItemPrefabModel>();
                }
            });
        }

        [DbPermissionAspect(Action = InteractionDbType.CREATE, Table = DbTable.StorageCellPrefab)]
        public async Task<StorageCellPrefabModel> CreateStorageCellPrefab(AuthorizedDto<StorageCellPrefabDto> dto)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    StorageCellPrefabEntity storageCellPrefabEntity = dto.Data.ToModel<StorageCellPrefabModel>().ToEntity<StorageCellPrefabEntity>();

                    await db.GetRepo<StorageCellPrefabEntity>().Create(storageCellPrefabEntity);
                    await db.Save();

                    return storageCellPrefabEntity.ToModel<StorageCellPrefabModel>();
                }
            });
        }

        [DbPermissionAspect(Action = InteractionDbType.CREATE, Table = DbTable.DetectorPrefab)]
        [DbPermissionAspect(Action = InteractionDbType.CREATE, Table = DbTable.DetectorDataPrefab)]
        [DbPermissionAspect(Action = InteractionDbType.CREATE, Table = DbTable.DetectorFaultPrefab)]
        [DbPermissionAspect(Action = InteractionDbType.CREATE, Table = DbTable.DetectorSettingsPrefab)]
        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.VisualizerType)]
        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.DataType)]
        public async Task<DetectorPrefabModel> CreateDetectorPrefab(AuthorizedDto<DetectorPrefabDto> dto)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    DetectorPrefabEntity detectorPrefabEntity = dto.Data.ToModel<DetectorPrefabModel>().ToEntity<DetectorPrefabEntity>();

                    await db.GetRepo<DetectorPrefabEntity>().Create(detectorPrefabEntity);
                    await db.Save();

                    return detectorPrefabEntity.ToModel<DetectorPrefabModel>();
                }
            });
        }
    }
}
