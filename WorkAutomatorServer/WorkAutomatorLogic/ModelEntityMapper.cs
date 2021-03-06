using System.Linq;
using System.Collections.Generic;

using AutoMapper;

using Constants;

using WorkAutomatorDataAccess.Entities;

using WorkAutomatorLogic.Models;
using WorkAutomatorLogic.Models.Prefabs;
using WorkAutomatorLogic.Models.Pipeline;
using WorkAutomatorLogic.Models.DetectorData;
using WorkAutomatorLogic.Models.Roles;
using WorkAutomatorLogic.Models.Event;
using WorkAutomatorLogic.Models.Permission;

namespace WorkAutomatorLogic
{
    public static class ModelEntityMapper
    {
        private static Mapper mapper;
        public static Mapper Mapper
        {
            get
            {
                if (mapper == null)
                    mapper = new Mapper(new MapperConfiguration(cnf => Configure(cnf)));
                return mapper;
            }
        }

        public static TEntity ToEntity<TEntity>(this ModelBase model) where TEntity : EntityBase
        {
            return Mapper.Map<TEntity>(model);
        }

        public static TModel ToModel<TModel>(this EntityBase entity) where TModel : ModelBase
        {
            return Mapper.Map<TModel>(entity);
        }

        private static void Configure(IMapperConfigurationExpression config)
        {
            config.CreateMap<PermissionDbModel, DbPermissionEntity>()
                  .ForMember(entity => entity.DbPermissionType, cnf => cnf.Ignore())
                  .ForMember(entity => entity.table_name, cnf => cnf.Ignore())
                  .ReverseMap()
                  .ForMember(model => model.InteractionDbType, cnf => cnf.ConvertUsing(new DbInteractionTypeToEnumConverter(), entity => entity.DbPermissionType.name))
                  .ForMember(model => model.DbTable, cnf => cnf.ConvertUsing(new TableNameToEnumConverter(), entity => entity.table_name));

            config.CreateMap<RoleModel, RoleEntity>()
                  .ForMember(entity => entity.company_id, cnf => cnf.MapFrom(model => model.CompanyId))
                  .ForMember(entity => entity.is_default, cnf => cnf.MapFrom(model => model.IsDefault))
                  .ReverseMap()
                  .ForMember(model => model.CompanyId, cnf => cnf.MapFrom(entity => entity.company_id))
                  .ForMember(model => model.IsDefault, cnf => cnf.MapFrom(entity => entity.is_default));

            config.CreateMap<AccountModel, AccountEntity>()
                  .ForMember(entity => entity.first_name, cnf => cnf.MapFrom(model => model.FirstName))
                  .ForMember(entity => entity.last_name, cnf => cnf.MapFrom(model => model.LastName))
                  .ReverseMap()
                  .ForMember(model => model.FirstName, cnf => cnf.MapFrom(entity => entity.first_name))
                  .ForMember(model => model.LastName, cnf => cnf.MapFrom(entity => entity.last_name));

            config.CreateMap<WorkerModel, AccountEntity>()
                  .ForMember(entity => entity.first_name, cnf => cnf.MapFrom(model => model.FirstName))
                  .ForMember(entity => entity.last_name, cnf => cnf.MapFrom(model => model.LastName))
                  .ForMember(entity => entity.company_id, cnf => cnf.MapFrom(model => model.CompanyId))
                  .ReverseMap()
                  .ForMember(model => model.CompanyId, cnf => cnf.MapFrom(entity => entity.company_id))
                  .ForMember(model => model.FirstName, cnf => cnf.MapFrom(entity => entity.first_name))
                  .ForMember(model => model.LastName, cnf => cnf.MapFrom(entity => entity.last_name));

            config.CreateMap<CompanyPlanPointModel, CompanyPlanUniquePointEntity>().ReverseMap();

            config.CreateMap<CompanyModel, CompanyEntity>()
                  .ForMember(entity => entity.owner_id, cnf => cnf.MapFrom(model => model.Id))
                  .ForMember(entity => entity.plan_image_url, cnf => cnf.MapFrom(model => model.PlanImageUrl))
                  .ReverseMap()
                  .ForMember(model => model.PlanImageUrl, cnf => cnf.MapFrom(entity => entity.plan_image_url))
                  .ForMember(model => model.Id, cnf => cnf.MapFrom(entity => entity.owner_id));

            config.CreateMap<ManufactoryPlanPointModel, ManufactoryPlanPointEntity>()
                  .ForMember(entity => entity.manufactory_id, cnf => cnf.MapFrom(model => model.ManufactoryId))
                  .ForMember(entity => entity.company_plan_unique_point_id, cnf => cnf.MapFrom(model => model.CompanyPlanPointId))
                  .ReverseMap()
                  .ForMember(model => model.ManufactoryId, cnf => cnf.MapFrom(entity => entity.manufactory_id))
                  .ForMember(model => model.CompanyPlanPointId, cnf => cnf.MapFrom(entity => entity.company_plan_unique_point_id));

            config.CreateMap<ManufactoryModel, ManufactoryEntity>()
                  .ForMember(entity => entity.company_id, cnf => cnf.MapFrom(model => model.CompanyId))
                  .ReverseMap()
                  .ForMember(model => model.CompanyId, cnf => cnf.MapFrom(entity => entity.company_id));

            config.CreateMap<CheckPointModel, CheckPointEntity>().ReverseMap();
            config.CreateMap<EnterLeavePointModel, EnterLeavePointEntity>().ReverseMap();

            config.CreateMap<DataTypeModel, DataTypeEntity>().ReverseMap();
            config.CreateMap<VisualizerTypeModel, VisualizerTypeEntity>().ReverseMap();

            config.CreateMap<PipelineItemSettingsPrefabModel, PipelineItemSettingsPrefabEntity>()
                  .ForMember(entity => entity.option_name, cnf => cnf.MapFrom(model => model.OptionName))
                  .ForMember(entity => entity.option_data_type_id, cnf => cnf.MapFrom(model => model.OptionDataType.Id))
                  .ForMember(entity => entity.option_description, cnf => cnf.MapFrom(model => model.OptionDescription))
                  .ReverseMap()
                  .ForMember(model => model.OptionName, cnf => cnf.MapFrom(entity => entity.option_name))
                  .ForMember(model => model.OptionDataType, cnf => cnf.MapFrom(entity => entity.DataType))
                  .ForMember(model => model.OptionDescription, cnf => cnf.MapFrom(entity => entity.option_description));

            config.CreateMap<PipelineItemPrefabModel, PipelineItemPrefabEntity>()
                  .ForMember(entity => entity.company_id, cnf => cnf.MapFrom(model => model.CompanyId))
                  .ForMember(entity => entity.image_url, cnf => cnf.MapFrom(model => model.ImageUrl))
                  .ForMember(entity => entity.input_x, cnf => cnf.MapFrom(model => model.InputX))
                  .ForMember(entity => entity.input_y, cnf => cnf.MapFrom(model => model.InputY))
                  .ForMember(entity => entity.output_x, cnf => cnf.MapFrom(model => model.OutputX))
                  .ForMember(entity => entity.output_y, cnf => cnf.MapFrom(model => model.OutputY))
                  .ReverseMap()
                  .ForMember(model => model.CompanyId, cnf => cnf.MapFrom(entity => entity.company_id))
                  .ForMember(model => model.ImageUrl, cnf => cnf.MapFrom(entity => entity.image_url))
                  .ForMember(model => model.InputX, cnf => cnf.MapFrom(entity => entity.input_x))
                  .ForMember(model => model.InputY, cnf => cnf.MapFrom(entity => entity.input_y))
                  .ForMember(model => model.OutputX, cnf => cnf.MapFrom(entity => entity.output_x))
                  .ForMember(model => model.OutputY, cnf => cnf.MapFrom(entity => entity.output_y));

            config.CreateMap<StorageCellPrefabModel, StorageCellPrefabEntity>()
                  .ForMember(entity => entity.company_id, cnf => cnf.MapFrom(model => model.CompanyId))
                  .ForMember(entity => entity.image_url, cnf => cnf.MapFrom(model => model.ImageUrl))
                  .ForMember(entity => entity.input_x, cnf => cnf.MapFrom(model => model.InputX))
                  .ForMember(entity => entity.input_y, cnf => cnf.MapFrom(model => model.InputY))
                  .ForMember(entity => entity.output_x, cnf => cnf.MapFrom(model => model.OutputX))
                  .ForMember(entity => entity.output_y, cnf => cnf.MapFrom(model => model.OutputY))
                  .ReverseMap()
                  .ForMember(model => model.CompanyId, cnf => cnf.MapFrom(entity => entity.company_id))
                  .ForMember(model => model.ImageUrl, cnf => cnf.MapFrom(entity => entity.image_url))
                  .ForMember(model => model.InputX, cnf => cnf.MapFrom(entity => entity.input_x))
                  .ForMember(model => model.InputY, cnf => cnf.MapFrom(entity => entity.input_y))
                  .ForMember(model => model.OutputX, cnf => cnf.MapFrom(entity => entity.output_x))
                  .ForMember(model => model.OutputY, cnf => cnf.MapFrom(entity => entity.output_y));

            config.CreateMap<DetectorDataPrefabModel, DetectorDataPrefabEntity>()
                  .ForMember(entity => entity.field_name, cnf => cnf.MapFrom(model => model.FieldName))
                  .ForMember(entity => entity.field_description, cnf => cnf.MapFrom(model => model.FieldDescription))
                  .ForMember(entity => entity.field_data_type_id, cnf => cnf.MapFrom(model => model.FieldDataType.Id))
                  .ForMember(entity => entity.argument_name, cnf => cnf.MapFrom(model => model.ArgumentName))
                  .ForMember(entity => entity.visualizer_type_id, cnf => cnf.MapFrom(model => model.VisualizerType.Id))
                  .ForMember(entity => entity.visualizer_type, cnf => cnf.Ignore())
                  .ReverseMap()
                  .ForMember(model => model.FieldName, cnf => cnf.MapFrom(entity => entity.field_name))
                  .ForMember(model => model.FieldDescription, cnf => cnf.MapFrom(entity => entity.field_description))
                  .ForMember(model => model.FieldDataType, cnf => cnf.MapFrom(entity => entity.DataType))
                  .ForMember(model => model.ArgumentName, cnf => cnf.MapFrom(entity => entity.argument_name))
                  .ForMember(model => model.VisualizerType, cnf => cnf.MapFrom(entity => entity.visualizer_type));

            config.CreateMap<DetectorFaultPrefabModel, DetectorFaultPrefabEntity>()
                  .ForMember(entity => entity.fault_condition, cnf => cnf.MapFrom(model => model.FaultCondition))
                  .ForMember(entity => entity.image_url, cnf => cnf.MapFrom(model => model.ImageUrl))
                  .ReverseMap()
                  .ForMember(model => model.ImageUrl, cnf => cnf.MapFrom(entity => entity.image_url));

            config.CreateMap<DetectorSettingsPrefabModel, DetectorSettingsPrefabEntity>()
                  .ForMember(entity => entity.option_name, cnf => cnf.MapFrom(model => model.OptionName))
                  .ForMember(entity => entity.option_description, cnf => cnf.MapFrom(model => model.OptionDescription))
                  .ForMember(entity => entity.option_data_type_id, cnf => cnf.MapFrom(model => model.OptionDataType.Id))
                  .ReverseMap()
                  .ForMember(model => model.OptionName, cnf => cnf.MapFrom(entity => entity.option_name))
                  .ForMember(model => model.OptionDescription, cnf => cnf.MapFrom(entity => entity.option_description))
                  .ForMember(model => model.OptionDataType, cnf => cnf.MapFrom(entity => entity.DataType));

            config.CreateMap<DetectorPrefabModel, DetectorPrefabEntity>()
                  .ForMember(entity => entity.company_id, cnf => cnf.MapFrom(model => model.CompanyId))
                  .ForMember(entity => entity.image_url, cnf => cnf.MapFrom(model => model.ImageUrl))
                  .ReverseMap()
                  .ForMember(model => model.CompanyId, cnf => cnf.MapFrom(entity => entity.company_id))
                  .ForMember(model => model.ImageUrl, cnf => cnf.MapFrom(entity => entity.image_url));

            config.CreateMap<PipelineItemSettingsValueModel, PipelineItemSettingsValueEntity>()
                  .ForMember(entity => entity.pipeline_item_settings_prefab_id, cnf => cnf.MapFrom(model => model.Prefab.Id))
                  .ForMember(entity => entity.option_data_value_base64, cnf => cnf.MapFrom(model => model.ValueBase64))
                  .ReverseMap()
                  .ForMember(model => model.Prefab, cnf => cnf.MapFrom(entity => entity.PipelineItemSettingsPrefab))
                  .ForMember(model => model.ValueBase64, cnf => cnf.MapFrom(entity => entity.option_data_value_base64));

            config.CreateMap<PipelineItemModel, PipelineItemEntity>()
                  .ForMember(entity => entity.PipelineItemSettingsValues, cnf => cnf.MapFrom(model => model.SettingsValues))
                  .ForMember(entity => entity.pipeline_item_prefab_id, cnf => cnf.MapFrom(model => model.Prefab.Id))
                  .ReverseMap()
                  .ForMember(model => model.SettingsValues, cnf => cnf.MapFrom(entity => entity.PipelineItemSettingsValues))
                  .ForMember(model => model.Prefab, cnf => cnf.MapFrom(entity => entity.PipelineItemPrefab))
                  .ForMember(model => model.ManufactoryId, cnf => cnf.MapFrom(entity => entity.manufactory_id))
                  .ForMember(model => model.PipelineId, cnf => cnf.MapFrom(entity => entity.pipeline_id));

            config.CreateMap<StorageCellModel, StorageCellEntity>()
                 .ForMember(entity => entity.storage_cell_prefab_id, cnf => cnf.MapFrom(model => model.Prefab.Id))
                 .ReverseMap()
                 .ForMember(model => model.Prefab, cnf => cnf.MapFrom(entity => entity.StorageCellPrefab))
                 .ForMember(model => model.ManufactoryId, cnf => cnf.MapFrom(entity => entity.manufactory_id))
                 .ForMember(model => model.PipelineId, cnf => cnf.MapFrom(entity => entity.pipeline_id));

            config.CreateMap<DetectorSettingsValueModel, DetectorSettingsValueEntity>()
                  .ForMember(entity => entity.detector_settings_prefab_id, cnf => cnf.MapFrom(model => model.Prefab.Id))
                  .ForMember(entity => entity.option_data_value_base64, cnf => cnf.MapFrom(model => model.ValueBase64))
                  .ReverseMap()
                  .ForMember(model => model.Prefab, cnf => cnf.MapFrom(entity => entity.detector_settings_prefab))
                  .ForMember(model => model.ValueBase64, cnf => cnf.MapFrom(entity => entity.option_data_value_base64));

            config.CreateMap<DetectorModel, DetectorEntity>()
                  .ForMember(entity => entity.detector_prefab_id, cnf => cnf.MapFrom(model => model.Prefab.Id))
                  .ForMember(entity => entity.DetectorFaultPrefabs, cnf => cnf.MapFrom(model => model.TrackedDetectorFaults))
                  .ForMember(entity => entity.DetectorSettingsValues, cnf => cnf.MapFrom(model => model.SettingsValues))
                  .ReverseMap()
                  .ForMember(model => model.SettingsValues, cnf => cnf.MapFrom(entity => entity.DetectorSettingsValues))
                  .ForMember(model => model.Prefab, cnf => cnf.MapFrom(entity => entity.DetectorPrefab))
                  .ForMember(model => model.TrackedDetectorFaults, cnf => cnf.MapFrom(entity => entity.DetectorFaultPrefabs))
                  .ForMember(model => model.PipelineItemId, cnf => cnf.MapFrom(entity => entity.pipeline_item_id));

            config.CreateMap<DetectorFaultEventModel, DetectorFaultEventEntity>()
                  .ForMember(entity => entity.associated_task_id, cnf => cnf.MapFrom(model => model.Id))
                  .ForMember(entity => entity.detector_id, cnf => cnf.MapFrom(model => model.Detector.Id))
                  .ForMember(entity => entity.detector_fault_prefab_id, cnf => cnf.MapFrom(model => model.Fault.Id))
                  .ForMember(entity => entity.is_fixed, cnf => cnf.MapFrom(model => model.IsFixed))
                  .ReverseMap()
                  .ForMember(model => model.Detector, cnf => cnf.MapFrom(entity => entity.detector))
                  .ForMember(model => model.Fault, cnf => cnf.MapFrom(entity => entity.detector_fault_prefab))
                  .ForMember(model => model.IsFixed, cnf => cnf.MapFrom(entity => entity.is_fixed))
                  .ForMember(model => model.Id, cnf => cnf.MapFrom(entity => entity.associated_task_id));

            config.CreateMap<DetectorDataItemModel, DetectorDataEntity>()
                  .ForMember(entity => entity.detector_data_prefab, cnf => cnf.MapFrom(model => model.DataPrefab))
                  .ForMember(entity => entity.field_data_value_base64, cnf => cnf.MapFrom(model => model.DataBase64))
                  .ReverseMap()
                  .ForMember(model => model.DataPrefab, cnf => cnf.MapFrom(entity => entity.detector_data_prefab))
                  .ForMember(model => model.DataBase64, cnf => cnf.MapFrom(entity => entity.field_data_value_base64));

            config.CreateMap<TaskModel, TaskEntity>()
                  .ForMember(entity => entity.company_id, cnf => cnf.MapFrom(model => model.CompanyId))
                  .ForMember(entity => entity.is_done, cnf => cnf.MapFrom(model => model.IsDone))
                  .ForMember(entity => entity.is_reviewed, cnf => cnf.MapFrom(model => model.IsReviewed))
                  .ReverseMap()
                  .ForMember(model => model.CompanyId, cnf => cnf.MapFrom(entity => entity.company_id))
                  .ForMember(model => model.IsDone, cnf => cnf.MapFrom(entity => entity.is_done))
                  .ForMember(model => model.IsReviewed, cnf => cnf.MapFrom(entity => entity.is_reviewed))
                  .ForMember(model => model.AssociatedFaultId, cnf => cnf.MapFrom(entity => entity.AssociatedFault.associated_task_id))
                  .ForMember(model => model.AssociatedFaultPrefab, cnf => cnf.MapFrom(entity => entity.AssociatedFault.detector_fault_prefab));

            config.CreateMap<CheckPointEventModel, CheckPointEventEntity>()
                  .ForMember(entity => entity.check_point, cnf => cnf.MapFrom(model => model.CheckPoint))
                  .ForMember(entity => entity.is_direct, cnf => cnf.MapFrom(model => model.IsDirect))
                  .ReverseMap()
                  .ForMember(model => model.CheckPoint, cnf => cnf.MapFrom(entity => entity.check_point))
                  .ForMember(model => model.IsDirect, cnf => cnf.MapFrom(entity => entity.is_direct));

            config.CreateMap<EnterLeavePointEventModel, EnterLeavePointEventEntity>()
                  .ForMember(entity => entity.enter_leave_point, cnf => cnf.MapFrom(model => model.EnterLeavePoint))
                  .ForMember(entity => entity.is_enter, cnf => cnf.MapFrom(model => model.IsEnter))
                  .ReverseMap()
                  .ForMember(model => model.EnterLeavePoint, cnf => cnf.MapFrom(entity => entity.enter_leave_point))
                  .ForMember(model => model.IsEnter, cnf => cnf.MapFrom(entity => entity.is_enter));

            config.CreateMap<PipelineItemInteractionEventModel, PipelineItemInteractionEventEntity>()
                  .ForMember(entity => entity.pipeline_item, cnf => cnf.MapFrom(model => model.PipelineItem))
                  .ReverseMap()
                  .ForMember(model => model.PipelineItem, cnf => cnf.MapFrom(entity => entity.pipeline_item));

            config.CreateMap<StorageCellEventModel, StorageCellEventEntity>()
                  .ForMember(entity => entity.storage_cell, cnf => cnf.MapFrom(model => model.StorageCell))
                  .ReverseMap()
                  .ForMember(model => model.StorageCell, cnf => cnf.MapFrom(entity => entity.storage_cell));

            config.CreateMap<DetectorInteractionEventModel, DetectorInteractionEventEntity>().ReverseMap();
        }

        public static IReadOnlyDictionary<DbTable, string> TABLE_NAME_DICTIONARY = new Dictionary<DbTable, string>()
        {
            { DbTable.Account, "account" },
            { DbTable.Task, "task" },
            { DbTable.Company, "company" },
            { DbTable.CompanyPlanUniquePoint, "company_plan_unique_point" },
            { DbTable.CheckPoint, "check_point" },
            { DbTable.CheckPointEvent, "check_point_event" },
            { DbTable.Manufactory, "manufactory" },
            { DbTable.EnterLeavePoint, "enter_leave_point" },
            { DbTable.EnterLeavePointEvent, "enter_leave_point_event" },
            { DbTable.ManufactoryPlanPoint, "manufactory_plan_point" },
            { DbTable.Role, "role" },
            { DbTable.DbPermission, "db_permission" },
            { DbTable.DbPermissionType, "db_permission_type" },
            { DbTable.Detector, "detector" },
            { DbTable.DetectorInteractionEvent, "detector_interaction_event" },
            { DbTable.DetectorData, "detector_data" },
            { DbTable.DetectorDataPrefab, "detector_data_prefab" },
            { DbTable.DataType, "data_type" },
            { DbTable.DetectorSettingsPrefab, "detector_settings_prefab" },
            { DbTable.DetectorPrefab, "detector_prefab" },
            { DbTable.DetectorFaultPrefab, "detector_fault_prefab" },
            { DbTable.DetectorFaultEvent, "detector_fault_event" },
            { DbTable.DetectorSettingsValue, "detector_settings_value" },
            { DbTable.PipelineItemSettingsPrefab, "pipeline_item_settings_prefab" },
            { DbTable.PipelineItemPrefab, "pipeline_item_prefab" },
            { DbTable.PipelineItem, "pipeline_item" },
            { DbTable.Pipeline, "pipeline" },
            { DbTable.PipelineItemInteractionEvent, "pipeline_item_interaction_event" },
            { DbTable.PipelineItemSettingsValue, "pipeline_item_settings_value" },
            { DbTable.StorageCell, "storage_cell" },
            { DbTable.ResourceStorageCell, "resource_storage_cell" },
            { DbTable.Resource, "resource" },
            { DbTable.Unit, "unit" },
            { DbTable.StorageCellEvent, "storage_cell_event" },
            { DbTable.StorageCellPrefab, "storage_cell_prefab" },
            { DbTable.VisualizerType, "visualizer_type" },
            { DbTable.RoleDbPermission, "role_db_permission" },
            { DbTable.RolePipelineItemPermission, "role_pipeline_item_permission" },
            { DbTable.RoleStorageCellPermission, "role_storage_cell_permission" },
            { DbTable.RoleDetectorPermission, "role_detector_permission" },
            { DbTable.RoleManufactoryPermission, "role_manufactory_permission" },
            { DbTable.AccountBossesSubs, "account_bosses_subs" },
            { DbTable.AccountRole, "account_role" }
        };

        public static IReadOnlyDictionary<InteractionDbType, string> INTERACTION_DB_TYPES = new Dictionary<InteractionDbType, string>()
        {
            { InteractionDbType.READ, "READ" },
            { InteractionDbType.CREATE, "CREATE" },
            { InteractionDbType.UPDATE, "UPDATE" },
            { InteractionDbType.DELETE, "DELETE" }
        };

        public static IReadOnlyDictionary<DefaultRoles, string> DEFAULT_ROLES_NAMES = new Dictionary<DefaultRoles, string>()
        {
            { DefaultRoles.AUTHORIZED, "AUTHORIZED" },
            { DefaultRoles.HIRED, "HIRED" },
            { DefaultRoles.OWNER, "OWNER" },
            { DefaultRoles.SUPERADMIN, "SUPERADMIN" }
        };

        public static IReadOnlyDictionary<DataType, string> DATA_TYPES = new Dictionary<DataType, string>()
        {
            { DataType.INT, "int" },
            { DataType.INT_ARR, "int[]" },
            { DataType.FLOAT, "float" },
            { DataType.FLOAT_ARR, "float[]" },
            { DataType.BOOL, "bool" },
            { DataType.BOOL_ARR, "bool[]" }
        };

        public static IReadOnlyDictionary<VisualizerType, string> VISUALIZER_TYPES = new Dictionary<VisualizerType, string>()
        {
            { VisualizerType.SIGNAL, "SIGNAL" },
            { VisualizerType.VALUE, "VALUE" },
        };

        public static string ToName(this DbTable table)
        {
            return TABLE_NAME_DICTIONARY[table];
        }

        public static string ToName(this InteractionDbType interactionDbType)
        {
            return INTERACTION_DB_TYPES[interactionDbType];
        }

        public static string ToName(this DefaultRoles role)
        {
            return DEFAULT_ROLES_NAMES[role];
        }

        public static string ToName(this DataType dataType)
        {
            return DATA_TYPES[dataType];
        }

        public static DataType FromName(this string dataTypeName)
        {
            return DATA_TYPES.First(dt => dt.Value == dataTypeName).Key;
        }

        public static string ToName(this VisualizerType visualizerType)
        {
            return VISUALIZER_TYPES[visualizerType];
        }

        private class DbInteractionTypeToEnumConverter : IValueConverter<string, InteractionDbType>
        {
            public InteractionDbType Convert(string sourceMember, ResolutionContext context)
            {
                return INTERACTION_DB_TYPES.First(i => i.Value == sourceMember).Key;
            }
        }

        private class TableNameToEnumConverter : IValueConverter<string, DbTable>
        {
            public DbTable Convert(string sourceMember, ResolutionContext context)
            {
                return TABLE_NAME_DICTIONARY.First(i => i.Value == sourceMember).Key;
            }
        }
    }
}
