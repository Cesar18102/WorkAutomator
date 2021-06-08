using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Autofac;

using Constants;

using Dto;
using Dto.Pipeline;
using Dto.Interaction;
using Dto.DetectorData;

using WorkAutomatorDataAccess;
using WorkAutomatorDataAccess.Entities;

using WorkAutomatorLogic.Aspects;
using WorkAutomatorLogic.Exceptions;
using WorkAutomatorLogic.Models.DetectorData;
using WorkAutomatorLogic.Models.Pipeline;
using WorkAutomatorLogic.ServiceInterfaces;

namespace WorkAutomatorLogic.Services
{
    internal class DetectorService : ServiceBase, IDetectorService
    {
        private static RoleService RoleService = LogicDependencyHolder.Dependencies.Resolve<RoleService>();
        private static DataTypeService DataTypeService = LogicDependencyHolder.Dependencies.Resolve<DataTypeService>();
        private static FaultConditionParseService FaultConditionParseService = LogicDependencyHolder.Dependencies.Resolve<FaultConditionParseService>();

        [DbPermissionAspect(Action = InteractionDbType.CREATE, Table = DbTable.Detector)]
        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.DetectorPrefab, CheckSameCompany = true)]
        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.DetectorFaultPrefab, CheckSameCompany = true)]
        public async Task<DetectorModel> Create(AuthorizedDto<DetectorDto> dto)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    DetectorEntity detectorEntity = dto.Data.ToModel<DetectorModel>().ToEntity<DetectorEntity>();
                    DetectorPrefabEntity detectorPrefabEntity = await db.GetRepo<DetectorPrefabEntity>().Get(detectorEntity.detector_prefab_id);

                    detectorEntity.DetectorSettingsValues = null;

                    detectorEntity.DetectorFaultPrefabs = detectorEntity.DetectorFaultPrefabs.Select(
                        faultPrefab => detectorPrefabEntity.DetectorFaultPrefabs.FirstOrDefault(fp => fp.id == faultPrefab.id)
                    ).Where(fp => fp != null).ToList();

                    await db.GetRepo<DetectorEntity>().Create(detectorEntity);
                    await db.Save();

                    RoleEntity ownerRole = await RoleService.GetCompanyOwnerRole(detectorPrefabEntity.company_id, db);
                    ownerRole.DetectorPermissions.Add(detectorEntity);

                    RoleEntity creatorRole = await RoleService.GetCompanyWorkerRole(dto.Session.UserId, db);
                    creatorRole?.DetectorPermissions.Add(detectorEntity);

                    await db.Save();

                    return detectorEntity.ToModel<DetectorModel>();
                }
            });
        }

        [PermissionAspect(Type = InteractionType.DETECTOR)]
        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.Detector, CheckSameCompany = true)]
        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.DetectorSettingsPrefab, CheckSameCompany = true)]
        [DbPermissionAspect(Action = InteractionDbType.CREATE | InteractionDbType.UPDATE | InteractionDbType.DELETE, Table = DbTable.DetectorSettingsValue, CheckSameCompany = true)]
        public async Task<DetectorModel> SetupSettings(AuthorizedDto<DetectorDto> dto)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    IRepo<DetectorEntity> detectorRepo = db.GetRepo<DetectorEntity>();

                    DetectorEntity detectorEntity = await detectorRepo.FirstOrDefault(pi => pi.id == dto.Data.Id.Value);

                    DetectorInteractionEventEntity detectorEvent = new DetectorInteractionEventEntity()
                    {
                        account_id = dto.Session.UserId,
                        detector_id = detectorEntity.id,
                        timespan = DateTime.Now,
                        log = $"Settings setup for Detector #{detectorEntity.id} by {dto.Session.UserId}"
                    };

                    await db.GetRepo<DetectorInteractionEventEntity>().Create(detectorEvent);

                    foreach (DetectorSettingsValueDto settingsValueDto in dto.Data.SettingsValues)
                    {
                        if (settingsValueDto.Id.HasValue)
                        {
                            DetectorSettingsValueEntity settingsValueEntity = detectorEntity.DetectorSettingsValues.First(
                                setting => setting.id == settingsValueDto.Id.Value
                            );

                            DataType dataType = settingsValueEntity.detector_settings_prefab.DataType.name.FromName();
                            DataTypeService.CheckIsDataOfType(settingsValueDto.ValueBase64, dataType);
                            
                            settingsValueEntity.option_data_value_base64 = settingsValueDto.ValueBase64;
                        }
                        else
                        {
                            DetectorSettingsPrefabEntity detectorSettingsPrefab = detectorEntity.DetectorPrefab.DetectorSettingsPrefabs.FirstOrDefault(
                                settingsPrefab => settingsPrefab.id == settingsValueDto.PrefabId.Value
                            );

                            if (detectorSettingsPrefab == null)
                                throw new NotFoundException("DetectorSettingsPrefab");

                            DataType dataType = detectorSettingsPrefab.DataType.name.FromName();
                            DataTypeService.CheckIsDataOfType(settingsValueDto.ValueBase64, dataType);

                            DetectorSettingsValueEntity settingWithSameSettingPrefab = detectorEntity.DetectorSettingsValues.FirstOrDefault(
                                setting => setting.detector_settings_prefab_id == settingsValueDto.PrefabId.Value
                            );

                            if (settingWithSameSettingPrefab != null)
                                settingWithSameSettingPrefab.option_data_value_base64 = settingsValueDto.ValueBase64;
                            else
                            {
                                DetectorSettingsValueEntity settingsValueEntity = new DetectorSettingsValueEntity()
                                {
                                    detector_id = detectorEntity.id,
                                    option_data_value_base64 = settingsValueDto.ValueBase64,
                                    detector_settings_prefab_id = settingsValueDto.PrefabId.Value
                                };

                                detectorEntity.DetectorSettingsValues.Add(settingsValueEntity);
                            }
                        }
                    }

                    await db.Save();
                    return detectorEntity.ToModel<DetectorModel>();
                }
            });
        }

        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.Detector, CheckSameCompany = true)]
        public async Task<ICollection<DetectorModel>> Get(AuthorizedDto<CompanyDto> dto)
        {
            return await Execute(async () =>
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    IList<DetectorEntity> detectors = await db.GetRepo<DetectorEntity>().Get(
                        s => s.DetectorPrefab.company_id == dto.Data.CompanyId.Value
                    );

                    return ModelEntityMapper.Mapper.Map<IList<DetectorModel>>(detectors);
                }
            });
        }

        public async Task ProvideData(DetectorDataDto dto)
        {
            await Execute(async () =>
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    IRepo<DetectorDataEntity> detectorDataRepo = db.GetRepo<DetectorDataEntity>();
                    IRepo<DetectorFaultEventEntity> detectorFaultEventRepo = db.GetRepo<DetectorFaultEventEntity>();

                    DetectorEntity detector = await db.GetRepo<DetectorEntity>().FirstOrDefault(
                        d => d.id == dto.DetectorId.Value
                    );

                    if (detector == null)
                        throw new NotFoundException("Detector");

                    if (detector.PipelineItem == null)
                        throw new NotFoundException("Pipeline item");

                    List<DetectorDataEntity> datas = new List<DetectorDataEntity>();

                    foreach (DetectorDataItemDto dataItem in dto.Data)
                    {
                        DetectorDataPrefabEntity dataPrefab = detector.DetectorPrefab.DetectorDataPrefabs.FirstOrDefault(
                            dp => dp.id == dataItem.DetectorDataPrefabId.Value
                        );

                        if (dataPrefab == null)
                            throw new NotFoundException("Detector data prefab");

                        DataType dataType = dataPrefab.DataType.name.FromName();
                        DataTypeService.CheckIsDataOfType(dataItem.DataBase64, dataType);

                        DetectorDataEntity dataEntity = new DetectorDataEntity()
                        {
                            detector_id = detector.id,
                            detector_data_prefab_id = dataPrefab.id,
                            field_data_value_base64 = dataItem.DataBase64,
                            timespan = DateTime.Now
                        };

                        DetectorDataEntity created = await detectorDataRepo.Create(dataEntity);

                        datas.Add(created);
                    }

                    foreach(DetectorFaultPrefabEntity faultPrefab in detector.DetectorFaultPrefabs)
                    {
                        bool isFaultOccured = await FaultConditionParseService.ParseCondition(
                            faultPrefab, 
                            datas.ToArray(), 
                            detector.DetectorSettingsValues.ToArray(), 
                            detector.PipelineItem.PipelineItemSettingsValues.ToArray()
                        );

                        if (isFaultOccured)
                        {
                            int? assigneeAccountId = detector.DetectorPrefab.company.Members.FirstOrDefault(
                                member => member.Roles.SelectMany(r => r.PipelineItemPermissions)
                                    .SelectMany(pip => pip.Detectors).Contains(detector)
                            )?.id;

                            int? reviewerAccountId = detector.DetectorPrefab.company.Members.Where(m => m.id != assigneeAccountId).FirstOrDefault(
                                member => member.Roles.SelectMany(r => r.PipelineItemPermissions)
                                    .SelectMany(pip => pip.Detectors).Contains(detector)
                            )?.id;

                            TaskEntity associatedTask = new TaskEntity()
                            {
                                company_id = detector.DetectorPrefab.company_id,
                                name = $"Fix a fault \"{faultPrefab.name}\" on pipeline item #{detector.pipeline_item_id} \"{detector.PipelineItem.PipelineItemPrefab.name}\"",
                                assignee_account_id = assigneeAccountId,
                                reviewer_account_id = reviewerAccountId
                            };
                            
                            DetectorFaultEventEntity faultEvent = new DetectorFaultEventEntity()
                            {
                                detector_id = detector.id,
                                detector_fault_prefab_id = faultPrefab.id,
                                timespan = DateTime.Now,
                                is_fixed = false,
                                AssociatedTask = associatedTask
                            };

                            await detectorFaultEventRepo.Create(faultEvent);
                        }
                    }

                    await db.Save();
                }
            });
        }

        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.DetectorData, CheckSameCompany = true)]
        public async Task<DetectorDataModel> GetData(AuthorizedDto<GetDetectorDataDto> dto)
        {
            return await Execute(async () =>
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    DetectorEntity detector = await db.GetRepo<DetectorEntity>().FirstOrDefault(
                        d => d.id == dto.Data.DetectorId.Value
                    );

                    DetectorDataEntity[] datas = detector.DetectorDatas.Where(
                        data => data.timespan >= dto.Data.DateSince && data.timespan <= dto.Data.DateUntil
                    ).ToArray();

                    return new DetectorDataModel()
                    {
                        Detector = detector.ToModel<DetectorModel>(),
                        Data = ModelEntityMapper.Mapper.Map<ICollection<DetectorDataItemModel>>(datas)
                    };
                }
            });
        }

        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.DetectorFaultEvent, CheckSameCompany = true)]
        public async Task<ICollection<DetectorFaultEventModel>> GetActualFaults(AuthorizedDto<DetectorDto> dto)
        {
            return await Execute(async () =>
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    DetectorEntity detector = await db.GetRepo<DetectorEntity>().FirstOrDefault(
                        d => d.id == dto.Data.DetectorId.Value
                    );

                    DetectorFaultEventEntity[] actualFaults = detector.detector_fault_events.Where(
                        faultEvent => !faultEvent.is_fixed
                    ).ToArray();

                    return ModelEntityMapper.Mapper.Map<ICollection<DetectorFaultEventModel>>(actualFaults);
                }
            });
        }

        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.DetectorFaultEvent, CheckSameCompany = true)]
        public async Task<ICollection<DetectorFaultEventModel>> GetAllFaults(AuthorizedDto<GetDetectorDataDto> dto)
        {
            return await Execute(async () =>
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    DetectorEntity detector = await db.GetRepo<DetectorEntity>().FirstOrDefault(
                        d => d.id == dto.Data.DetectorId.Value
                    );

                    DetectorFaultEventEntity[] faults = detector.detector_fault_events.Where(
                        faultEvent => faultEvent.timespan >= dto.Data.DateSince && faultEvent.timespan <= dto.Data.DateUntil
                    ).ToArray();

                    return ModelEntityMapper.Mapper.Map<ICollection<DetectorFaultEventModel>>(faults);
                }
            });
        }

        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.Detector, CheckSameCompany = true)]
        public async Task TryInteract(DetectorInteractionDto dto)
        {
            await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    DetectorEntity detector = await db.GetRepo<DetectorEntity>().Get(dto.DetectorId.Value);
                    AccountEntity account = await db.GetRepo<AccountEntity>().Get(dto.AccountId.Value);

                    DetectorInteractionEventEntity detectorEvent = new DetectorInteractionEventEntity()
                    {
                        account_id = account.id,
                        detector_id = detector.id,
                        timespan = DateTime.Now
                    };

                    NotPermittedException ex = null;

                    if (account.Roles.SelectMany(r => r.DetectorPermissions).Any(m => m.id == detector.id))
                        detectorEvent.log = $"Interaction with Detector #{detector.id} by Account #{account.id}: SUCCESS";
                    else
                    {
                        detectorEvent.log = $"Interaction with Detector #{detector.id} by Account #{account.id}: ACCESS DENIED";
                        ex = new NotPermittedException(detectorEvent.log);
                    }

                    await db.GetRepo<DetectorInteractionEventEntity>().Create(detectorEvent);
                    await db.Save();

                    if (ex != null)
                        throw ex;
                }
            });
        }
    }
}
