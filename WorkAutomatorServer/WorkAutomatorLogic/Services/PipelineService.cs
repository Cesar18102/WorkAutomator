using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Autofac;

using Constants;

using Dto;
using Dto.Pipeline;

using WorkAutomatorDataAccess;
using WorkAutomatorDataAccess.Entities;

using WorkAutomatorLogic.Aspects;
using WorkAutomatorLogic.Exceptions;
using WorkAutomatorLogic.Models.Pipeline;
using WorkAutomatorLogic.ServiceInterfaces;

namespace WorkAutomatorLogic.Services
{
    internal class PipelineService : ServiceBase, IPipelineService
    {
        private static IntersectionService IntersectionService = LogicDependencyHolder.Dependencies.Resolve<IntersectionService>();

        public bool IsPointInsideManufactory(ManufactoryEntity manufactory, double x, double y)
        {
            return IntersectionService.CheckInside(
                (x, y), manufactory.ManufactoryPlanPoints.Select(
                    p => (p.CompanyPlanUniquePoint.x, p.CompanyPlanUniquePoint.y)
                ).ToArray()
            );
        }

        private ManufactoryEntity GetManufactoryByPoint(CompanyEntity company, double x, double y)
        {
            return company.Manufactories.FirstOrDefault(
                manufactory => IsPointInsideManufactory(manufactory, x, y)
            );
        }

        private void UpdatePipelineItemPlacements(
            CompanyEntity company, ICollection<PipelineItemEntity> pipelineItems,
            ICollection<PipelineItemPlacementDto> pipelineItemPlacements
        )
        {
            foreach (PipelineItemPlacementDto pipelineItemPlacement in pipelineItemPlacements)
            {
                PipelineItemEntity pipelineItemEntity = pipelineItems.FirstOrDefault(
                    pi => pi.id == pipelineItemPlacement.PipelineItemId.Value
                );

                ManufactoryEntity manufactoryPlacement = GetManufactoryByPoint(
                    company, pipelineItemPlacement.X.Value, pipelineItemPlacement.Y.Value
                );

                if (manufactoryPlacement == null)
                    throw new PlacementException();

                pipelineItemEntity.manufactory_id = manufactoryPlacement.id;
                pipelineItemEntity.x = pipelineItemPlacement.X;
                pipelineItemEntity.y = pipelineItemPlacement.Y;
            }
        }

        private void UpdateStorageCellPlacements(
            CompanyEntity company, ICollection<StorageCellEntity> storageCells,
            ICollection<StorageCellPlacementDto> storageCellPlacements
        )
        {
            foreach (StorageCellPlacementDto storageCellPlacement in storageCellPlacements)
            {
                StorageCellEntity storageCellEntity = storageCells.FirstOrDefault(
                    pi => pi.id == storageCellPlacement.StorageCellId.Value
                );

                ManufactoryEntity manufactoryPlacement = GetManufactoryByPoint(
                    company, storageCellPlacement.X.Value, storageCellPlacement.Y.Value
                );

                if (manufactoryPlacement == null)
                    throw new PlacementException();

                storageCellEntity.manufactory_id = manufactoryPlacement.id;
                storageCellEntity.x = storageCellPlacement.X;
                storageCellEntity.y = storageCellPlacement.Y;
            }
        }

        private void UpdatePipelineConnections(
            ICollection<PipelineItemEntity> pipelineItems,
            ICollection<StorageCellEntity> storageCells,
            PipelineDto pipelineDto, PipelineEntity pipeline
        )
        {
            foreach (PipelineConnectionDto connection in pipelineDto.Connections)
            {
                PipelineItemEntity pipelineItemEntity = pipelineItems.FirstOrDefault(
                    pi => pi.id == connection.PipelineItemId.Value
                );
                pipelineItemEntity.pipeline_id = pipeline.id;

                PipelineItemEntity[] inputPipelineItems = connection.InputPipelineItems.Select(
                    inputPipelineItem => pipelineItems.FirstOrDefault(pi => pi.id == inputPipelineItem.Id.Value)
                ).ToArray();

                foreach (PipelineItemEntity pipelineItemToRemove in pipelineItemEntity.InputPipelineItems.Except(inputPipelineItems).ToArray())
                    pipelineItemEntity.InputPipelineItems.Remove(pipelineItemToRemove);

                foreach (PipelineItemEntity pipelineItemToAdd in inputPipelineItems.Except(pipelineItemEntity.InputPipelineItems).ToArray())
                    pipelineItemEntity.InputPipelineItems.Add(pipelineItemToAdd);


                PipelineItemEntity[] outputPipelineItems = connection.OutputPipelineItems.Select(
                    outputPipelineItem => pipelineItems.FirstOrDefault(pi => pi.id == outputPipelineItem.Id.Value)
                ).ToArray();

                foreach (PipelineItemEntity pipelineItemToRemove in pipelineItemEntity.OutputPipelineItems.Except(outputPipelineItems).ToArray())
                    pipelineItemEntity.OutputPipelineItems.Remove(pipelineItemToRemove);

                foreach (PipelineItemEntity pipelineItemToAdd in outputPipelineItems.Except(pipelineItemEntity.OutputPipelineItems).ToArray())
                    pipelineItemEntity.OutputPipelineItems.Add(pipelineItemToAdd);


                StorageCellEntity[] inputStorageCells = connection.InputStorageCells.Select(
                    inputStorageCell => storageCells.FirstOrDefault(pi => pi.id == inputStorageCell.Id.Value)
                ).ToArray();

                foreach (StorageCellEntity storageCell in inputStorageCells)
                    storageCell.pipeline_id = pipeline.id;

                foreach (StorageCellEntity storageCellToRemove in pipelineItemEntity.InputStorageCells.Except(inputStorageCells).ToArray())
                    pipelineItemEntity.InputStorageCells.Remove(storageCellToRemove);

                foreach (StorageCellEntity storageCellToAdd in inputStorageCells.Except(pipelineItemEntity.InputStorageCells).ToArray())
                    pipelineItemEntity.InputStorageCells.Add(storageCellToAdd);


                StorageCellEntity[] outputStorageCells = connection.OutputStorageCells.Select(
                    outputStorageCell => storageCells.FirstOrDefault(pi => pi.id == outputStorageCell.Id.Value)
                ).ToArray();

                foreach (StorageCellEntity storageCell in outputStorageCells)
                    storageCell.pipeline_id = pipeline.id;

                foreach (StorageCellEntity storageCellToRemove in pipelineItemEntity.OutputStorageCells.Except(outputStorageCells).ToArray())
                    pipelineItemEntity.OutputStorageCells.Remove(storageCellToRemove);

                foreach (StorageCellEntity storageCellToAdd in outputStorageCells.Except(pipelineItemEntity.OutputStorageCells).ToArray())
                    pipelineItemEntity.OutputStorageCells.Add(storageCellToAdd);
            }
        }

        public PipelineModel ToPipelineModel(PipelineEntity pipeline)
        {
            PipelineModel pipelineModel = new PipelineModel();

            pipelineModel.Id = pipeline.id;
            pipelineModel.CompanyId = pipeline.company_id;
            pipelineModel.Connections = new List<PipelineItemConnectionModel>();

            foreach (PipelineItemEntity pipelineItem in pipeline.PipelineItems)
            {
                PipelineItemConnectionModel connectionModel = new PipelineItemConnectionModel();

                connectionModel.PipelineItem = pipelineItem.ToModel<PipelineItemModel>();

                connectionModel.InputPipelineItems = ModelEntityMapper.Mapper.Map<ICollection<PipelineItemModel>>(
                    pipelineItem.InputPipelineItems
                );

                connectionModel.OutputPipelineItems = ModelEntityMapper.Mapper.Map<ICollection<PipelineItemModel>>(
                    pipelineItem.OutputPipelineItems
                );

                connectionModel.InputStorageCells = ModelEntityMapper.Mapper.Map<ICollection<StorageCellModel>>(
                    pipelineItem.InputStorageCells
                );

                connectionModel.OutputStorageCells = ModelEntityMapper.Mapper.Map<ICollection<StorageCellModel>>(
                    pipelineItem.OutputStorageCells
                );

                pipelineModel.Connections.Add(connectionModel);
            }

            return pipelineModel;
        }

        private void ValidatePipelineItems(IEnumerable<PipelineItemEntity> pipelineItems, int? pipelineId)
        {
            DataValidationException validationException = new DataValidationException();

            IEnumerable<PipelineItemEntity> usedPipelineItems = pipelineItems.Where(
                pipelineItem => pipelineItem.pipeline_id != null && pipelineItem.pipeline_id != pipelineId
            );

            foreach (PipelineItemEntity usedPipelineItem in usedPipelineItems)
            {
                validationException.Add(
                    typeof(PipelineDto), nameof(PipelineDto.Connections),
                    $"Pipeline item #{usedPipelineItem.id} is already used in some pipeline"
                );
            }

            if (validationException.InvalidFieldInfos.Count != 0)
                throw validationException;
        }

        private void ValidateStorageCells(IEnumerable<StorageCellEntity> storageCells, int? pipelineId)
        {
            DataValidationException validationException = new DataValidationException();

            IEnumerable<StorageCellEntity> usedStorageCells = storageCells.Where(
                storageCell => storageCell.pipeline_id != null && storageCell.pipeline_id != pipelineId
            );

            foreach (StorageCellEntity usedStorageCell in usedStorageCells)
            {
                validationException.Add(
                    typeof(PipelineDto), nameof(PipelineDto.Connections),
                    $"Storage cell #{usedStorageCell.id} is already used in some pipeline"
                );
            }

            if (validationException.InvalidFieldInfos.Count != 0)
                throw validationException;
        }

        [DbPermissionAspect(Action = InteractionDbType.CREATE, Table = DbTable.Pipeline)]
        [DbPermissionAspect(Action = InteractionDbType.READ | InteractionDbType.UPDATE, Table = DbTable.StorageCell, CheckSameCompany = true)]
        [DbPermissionAspect(Action = InteractionDbType.READ | InteractionDbType.UPDATE, Table = DbTable.PipelineItem, CheckSameCompany = true)]
        public async Task<PipelineModel> Create(AuthorizedDto<PipelineDto> dto)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    CompanyEntity company = await db.GetRepo<CompanyEntity>().FirstOrDefault(
                        c => c.owner_id == dto.Data.CompanyId.Value
                    );

                    IList<PipelineItemEntity> pipelineItems = await db.GetRepo<PipelineItemEntity>().Get(
                        pipelineItem => pipelineItem.PipelineItemPrefab.company_id == company.owner_id
                    );

                    IList<StorageCellEntity> storageCells = await db.GetRepo<StorageCellEntity>().Get(
                        storageCell => storageCell.StorageCellPrefab.company_id == company.owner_id
                    );

                    this.ValidatePipelineItems(pipelineItems, null);
                    this.ValidateStorageCells(storageCells, null);

                    this.UpdatePipelineItemPlacements(company, pipelineItems, dto.Data.PipelineItemPlacements);
                    this.UpdateStorageCellPlacements(company, storageCells, dto.Data.StorageCellPlacements);

                    PipelineEntity pipeline = new PipelineEntity();
                    pipeline.company_id = company.owner_id;

                    await db.GetRepo<PipelineEntity>().Create(pipeline);

                    this.UpdatePipelineConnections(pipelineItems, storageCells, dto.Data, pipeline);

                    await db.Save();

                    return ToPipelineModel(pipeline);
                }
            });
        }

        [DbPermissionAspect(Action = InteractionDbType.UPDATE, Table = DbTable.Pipeline, CheckSameCompany = true)]
        [DbPermissionAspect(Action = InteractionDbType.READ | InteractionDbType.UPDATE, Table = DbTable.StorageCell, CheckSameCompany = true)]
        [DbPermissionAspect(Action = InteractionDbType.READ | InteractionDbType.UPDATE, Table = DbTable.PipelineItem, CheckSameCompany = true)]
        public async Task<PipelineModel> Update(AuthorizedDto<PipelineDto> dto)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    PipelineEntity pipeline = await db.GetRepo<PipelineEntity>().FirstOrDefault(
                        p => p.id == dto.Data.Id.Value
                    );

                    IList<PipelineItemEntity> pipelineItems = await db.GetRepo<PipelineItemEntity>().Get(
                        pipelineItem => pipelineItem.PipelineItemPrefab.company_id == pipeline.company_id
                    );

                    IList<StorageCellEntity> storageCells = await db.GetRepo<StorageCellEntity>().Get(
                        storageCell => storageCell.StorageCellPrefab.company_id == pipeline.company_id
                    );

                    this.ValidatePipelineItems(pipelineItems, pipeline.id);
                    this.ValidateStorageCells(storageCells, pipeline.id);

                    this.UpdatePipelineItemPlacements(pipeline.Company, pipelineItems, dto.Data.PipelineItemPlacements);
                    this.UpdateStorageCellPlacements(pipeline.Company, storageCells, dto.Data.StorageCellPlacements);
                    this.UpdatePipelineConnections(pipelineItems, storageCells, dto.Data, pipeline);

                    await db.Save();

                    return ToPipelineModel(pipeline);
                }
            });
        }

        [DbPermissionAspect(Action = InteractionDbType.DELETE, Table = DbTable.Pipeline, CheckSameCompany = true)]
        public async Task Scrap(AuthorizedDto<PipelineDto> dto)
        {
            await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    PipelineEntity pipeline = await db.GetRepo<PipelineEntity>().FirstOrDefault(
                        p => p.id == dto.Data.Id.Value
                    );

                    foreach (PipelineItemEntity pipelineItem in pipeline.PipelineItems)
                    {
                        pipelineItem.pipeline_id = null;

                        pipelineItem.InputPipelineItems.Clear();
                        pipelineItem.OutputPipelineItems.Clear();
                        pipelineItem.InputStorageCells.Clear();
                        pipelineItem.OutputStorageCells.Clear();
                    }

                    foreach (StorageCellEntity storageCell in pipeline.StorageCells)
                        storageCell.pipeline_id = null;

                    await db.Save();
                }
            });
        }

        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.Pipeline, CheckSameCompany = true)]
        public async Task<PipelineModel[]> Get(AuthorizedDto<CompanyDto> dto)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    IList<PipelineEntity> pipelines = await db.GetRepo<PipelineEntity>().Get(
                        p => p.company_id == dto.Data.Id.Value
                    );

                    return pipelines.Select(pipeline => ToPipelineModel(pipeline)).ToArray();
                }
            });
        }

        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.Pipeline, CheckSameCompany = true)]
        public async Task<PipelineModel> Get(AuthorizedDto<PipelineDto> dto)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    PipelineEntity pipeline = await db.GetRepo<PipelineEntity>().Get(dto.Data.Id.Value);
                    return ToPipelineModel(pipeline);
                }
            });
        }
    }
}
