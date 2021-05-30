using System.Collections.Generic;
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
    internal class DetectorService : ServiceBase, IDetectorService
    {
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

                    return detectorEntity.ToModel<DetectorModel>();
                }
            });
        }

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

                    foreach (DetectorSettingsValueDto settingsValueDto in dto.Data.SettingsValues)
                    {
                        if (settingsValueDto.Id.HasValue)
                        {
                            detectorEntity.DetectorSettingsValues.First(
                                setting => setting.id == settingsValueDto.Id.Value
                            ).option_data_value_base64 = settingsValueDto.ValueBase64;
                        }
                        else
                        {
                            bool isValidSettingsPrefabId = detectorEntity.DetectorPrefab.DetectorSettingsPrefabs.Any(
                                settingsPrefab => settingsPrefab.id == settingsValueDto.PrefabId.Value
                            );

                            if (!isValidSettingsPrefabId)
                                throw new NotFoundException("DetectorSettingsPrefab");

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
    }
}
