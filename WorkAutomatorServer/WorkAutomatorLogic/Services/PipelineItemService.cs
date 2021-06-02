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
using WorkAutomatorLogic.Exceptions;
using WorkAutomatorLogic.Models.Pipeline;
using WorkAutomatorLogic.ServiceInterfaces;

namespace WorkAutomatorLogic.Services
{
    internal class PipelineItemService : ServiceBase, IPipelineItemService
    {
        private static RoleService RoleService = LogicDependencyHolder.Dependencies.Resolve<RoleService>();
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

                    RoleEntity ownerRole = await RoleService.GetCompanyOwnerRole(pipelineItemEntity.PipelineItemPrefab.company_id);
                    ownerRole.PipelineItemPermissions.Add(pipelineItemEntity);

                    RoleEntity creatorRole = await RoleService.GetCompanyWorkerRole(dto.Session.UserId);
                    creatorRole.PipelineItemPermissions.Add(pipelineItemEntity);

                    await db.Save();

                    return pipelineItemEntity.ToModel<PipelineItemModel>();
                }
            });
        }

        [PermissionAspect(Type = InteractionType.PIPELINE_ITEM)]
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

                    PipelineItemInteractionEventEntity pipelineItemEvent = new PipelineItemInteractionEventEntity()
                    {
                        account_id = dto.Session.UserId,
                        pipeline_item_id = pipelineItemEntity.id,
                        timespan = DateTime.Now,
                        log = $"Settings setup for Pipeline Item #{pipelineItemEntity.id} by {dto.Session.UserId}"
                    };

                    await db.GetRepo<PipelineItemInteractionEventEntity>().Create(pipelineItemEvent);

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

        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.Detector, CheckSameCompany = true)]
        public async Task TryInteract(PipelineItemInteractionDto dto)
        {
            await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    PipelineItemEntity pipelineItem = await db.GetRepo<PipelineItemEntity>().Get(dto.PipelineItemId.Value);
                    AccountEntity account = await db.GetRepo<AccountEntity>().Get(dto.AccountId.Value);

                    PipelineItemInteractionEventEntity pipelineItemEvent = new PipelineItemInteractionEventEntity()
                    {
                        account_id = account.id,
                        pipeline_item_id = pipelineItem.id,
                        timespan = DateTime.Now
                    };

                    NotPermittedException ex = null;

                    if (account.Roles.SelectMany(r => r.PipelineItemPermissions).Any(m => m.id == pipelineItem.id))
                        pipelineItemEvent.log = $"Interaction with Pipeline item #{pipelineItem.id} by Account #{account.id}: SUCCESS";
                    else
                    {
                        pipelineItemEvent.log = $"Interaction with Pipeline item #{pipelineItem.id} by Account #{account.id}: ACCESS DENIED";
                        ex = new NotPermittedException(pipelineItemEvent.log);
                    }

                    await db.GetRepo<PipelineItemInteractionEventEntity>().Create(pipelineItemEvent);
                    await db.Save();

                    if (ex != null)
                        throw ex;
                }
            });
        }
    }
}
