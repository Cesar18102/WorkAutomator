using System.Collections.Generic;

using AutoMapper;

using Constants;

using WorkAutomatorLogic.Models;
using WorkAutomatorDataAccess.Entities;

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
            config.CreateMap<AccountModel, AccountEntity>()
                  .ForMember(entity => entity.first_name, cnf => cnf.MapFrom(model => model.FirstName))
                  .ForMember(entity => entity.last_name, cnf => cnf.MapFrom(model => model.LastName))
                  .ReverseMap()
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
            { DbTable.DetectorFault, "detector_fault" },
            { DbTable.DetectorFaultEvent, "detector_fault_event" },
            { DbTable.DetectorSettingsValue, "detector_settings_value" },
            { DbTable.PipelineItemSettingsPrefab, "pipeline_item_settings_prefab" },
            { DbTable.PipelineItemPrefab, "pipeline_item_prefab" },
            { DbTable.PipelineItem, "pipeline_item" },
            { DbTable.PipelineItemConnection, "pipeline_item_connection" },
            { DbTable.Pipeline, "pipeline" },
            { DbTable.PipelineItemInteractionEvent, "pipeline_item_interaction_event" },
            { DbTable.PipelineItemSettingsValue, "pipeline_item_settings_value" },
            { DbTable.PipelineItemStorageConnection, "pipeline_item_storage_connection" },
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
            { DefaultRoles.OWNER, "OWNER" }
        };

        //public static IReadOnlyDictionary<Type, string> DATA_TYPES = new Dictionary<>

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
    }
}
