using System.Linq;
using System.Threading.Tasks;

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
        [DbPermissionAspect(Action = InteractionDbType.CREATE | InteractionDbType.UPDATE | InteractionDbType.DELETE, Table = DbTable.PipelineItemSettingsValue, CheckSameCompany = true)]
        public async Task<PipelineItemModel> SetupSettings(AuthorizedDto<PipelineItemDto> dto)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    IRepo<PipelineItemEntity> pipelineItemRepo = db.GetRepo<PipelineItemEntity>();

                    PipelineItemEntity pipelineItemEntity = await pipelineItemRepo.FirstOrDefault(pi => pi.id == dto.Data.Id.Value);

                    foreach (PipelineItemSettingsValueDto settingsValueDto in dto.Data.SettingsValues)
                    {
                        if (settingsValueDto.Id.HasValue)
                        {
                            pipelineItemEntity.PipelineItemSettingsValues.First(
                                setting => setting.id == settingsValueDto.Id.Value
                            ).option_data_value_base64 = settingsValueDto.ValueBase64;
                        }
                        else
                        {
                            bool isValidSettingsPrefabId = pipelineItemEntity.PipelineItemPrefab.PipelineItemSettingsPrefabs.Any(
                                settingsPrefab => settingsPrefab.id == settingsValueDto.PipelineItemSettingsPrefabId.Value
                            );

                            if (!isValidSettingsPrefabId)
                                throw new NotFoundException("PipelineItemSettingsPrefab");

                            PipelineItemSettingsValueEntity settingWithSameSettingPrefab = pipelineItemEntity.PipelineItemSettingsValues.FirstOrDefault(
                                setting => setting.pipeline_item_settings_prefab_id == settingsValueDto.PipelineItemSettingsPrefabId.Value
                            );

                            if (settingWithSameSettingPrefab != null)
                                settingWithSameSettingPrefab.option_data_value_base64 = settingsValueDto.ValueBase64;
                            else
                            {
                                PipelineItemSettingsValueEntity settingsValueEntity = new PipelineItemSettingsValueEntity()
                                {
                                    pipeline_item_id = pipelineItemEntity.id,
                                    option_data_value_base64 = settingsValueDto.ValueBase64,
                                    pipeline_item_settings_prefab_id = settingsValueDto.PipelineItemSettingsPrefabId.Value
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
    }
}
