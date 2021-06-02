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
    internal class PipelineItemService : ServiceBase, IPipelineItemService
    {
        private static DataTypeService DataTypeService = LogicDependencyHolder.Dependencies.Resolve<DataTypeService>();

        [DbPermissionAspect(Action = InteractionDbType.CREATE, Table = DbTable.PipelineItem)]
        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.PipelineItemPrefab, CheckSameCompany = true)]
        public async Task<PipelineItemModel> Create(AuthorizedDto<PipelineItemDto> dto)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    PipelineItemEntity pipelineItemEntity = dto.Data.ToModel<PipelineItemModel>().ToEntity<PipelineItemEntity>();
                    pipelineItemEntity.PipelineItemSettingsValues = null;

                    await db.GetRepo<PipelineItemEntity>().Create(pipelineItemEntity);
                    await db.Save();

                    return pipelineItemEntity.ToModel<PipelineItemModel>();
                }
            });
        }

        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.PipelineItem, CheckSameCompany = true)]
        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.PipelineItemSettingsPrefab, CheckSameCompany = true)]
        [DbPermissionAspect(Action = InteractionDbType.CREATE | InteractionDbType.UPDATE | InteractionDbType.DELETE, Table = DbTable.PipelineItemSettingsValue, CheckSameCompany = true)]
        public async Task<PipelineItemModel> SetupSettings(AuthorizedDto<PipelineItemDto> dto)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    IRepo<PipelineItemEntity> pipelineItemRepo = db.GetRepo<PipelineItemEntity>();

                    PipelineItemEntity pipelineItemEntity = await pipelineItemRepo.FirstOrDefault(pi => pi.id == dto.Data.Id.Value);

                    //CREATE EVENT

                    foreach (PipelineItemSettingsValueDto settingsValueDto in dto.Data.SettingsValues)
                    {
                        if (settingsValueDto.Id.HasValue)
                        {
                            PipelineItemSettingsValueEntity settingsValueEntity = pipelineItemEntity.PipelineItemSettingsValues.First(
                                setting => setting.id == settingsValueDto.Id.Value
                            );

                            DataType dataType = settingsValueEntity.PipelineItemSettingsPrefab.DataType.name.FromName();
                            DataTypeService.CheckIsDataOfType(settingsValueDto.ValueBase64, dataType);
                            
                            settingsValueEntity.option_data_value_base64 = settingsValueDto.ValueBase64;
                        }
                        else
                        {
                            PipelineItemSettingsPrefabEntity pipelineItemSettingsPrefab = pipelineItemEntity.PipelineItemPrefab.PipelineItemSettingsPrefabs.FirstOrDefault(
                                settingsPrefab => settingsPrefab.id == settingsValueDto.PrefabId.Value
                            );

                            if (pipelineItemSettingsPrefab == null)
                                throw new NotFoundException("PipelineItemSettingsPrefab");

                            DataType dataType = pipelineItemSettingsPrefab.DataType.name.FromName();
                            DataTypeService.CheckIsDataOfType(settingsValueDto.ValueBase64, dataType);

                            PipelineItemSettingsValueEntity settingWithSameSettingPrefab = pipelineItemEntity.PipelineItemSettingsValues.FirstOrDefault(
                                setting => setting.pipeline_item_settings_prefab_id == settingsValueDto.PrefabId.Value
                            );

                            if (settingWithSameSettingPrefab != null)
                                settingWithSameSettingPrefab.option_data_value_base64 = settingsValueDto.ValueBase64;
                            else
                            {
                                PipelineItemSettingsValueEntity settingsValueEntity = new PipelineItemSettingsValueEntity()
                                {
                                    pipeline_item_id = pipelineItemEntity.id,
                                    option_data_value_base64 = settingsValueDto.ValueBase64,
                                    pipeline_item_settings_prefab_id = settingsValueDto.PrefabId.Value
                                };

                                pipelineItemEntity.PipelineItemSettingsValues.Add(settingsValueEntity);
                            }
                        }
                    }

                    await db.Save();
                    return pipelineItemEntity.ToModel<PipelineItemModel>();
                }
            });
        }

        [DbPermissionAspect(Action = InteractionDbType.READ | InteractionDbType.UPDATE, Table = DbTable.PipelineItem, CheckSameCompany = true)]
        [DbPermissionAspect(Action = InteractionDbType.READ | InteractionDbType.UPDATE, Table = DbTable.Detector, CheckSameCompany = true)]
        public async Task<PipelineItemModel> SetupDetector(AuthorizedDto<SetupDetectorDto> dto)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    DetectorEntity detector = await db.GetRepo<DetectorEntity>().FirstOrDefault(d => d.id == dto.Data.DetectorId.Value);
                    detector.pipeline_item_id = dto.Data.PipelineItemId.Value;

                    await db.Save();

                    PipelineItemEntity pipelineItemEntity = await db.GetRepo<PipelineItemEntity>().FirstOrDefault(
                        p => p.id == dto.Data.PipelineItemId.Value
                    );

                    return pipelineItemEntity.ToModel<PipelineItemModel>();
                }
            });
        }

        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.PipelineItem, CheckSameCompany = true)]
        public async Task<ICollection<PipelineItemModel>> Get(AuthorizedDto<CompanyDto> dto)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    IList<PipelineItemEntity> pipelineItems = await db.GetRepo<PipelineItemEntity>().Get(
                        p => p.PipelineItemPrefab.company_id == dto.Data.CompanyId.Value
                    );

                    return ModelEntityMapper.Mapper.Map<IList<PipelineItemModel>>(pipelineItems);
                }
            });
        }
    }
}
