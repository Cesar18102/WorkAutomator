using AutoMapper;

using Dto;
using Dto.Prefabs;

using WorkAutomatorLogic.Models;
using WorkAutomatorLogic.Models.Prefabs;

namespace WorkAutomatorLogic
{
    public static class ModelDtoMapper
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

        public static TModel ToModel<TModel>(this DtoBase dto) where TModel : ModelBase
        {
            return Mapper.Map<TModel>(dto);
        }

        public static TDto ToDto<TDto>(this ModelBase model) where TDto : DtoBase
        {
            return Mapper.Map<TDto>(model);
        }

        private static void Configure(IMapperConfigurationExpression config)
        {
            config.CreateMap<PublicKeyDto, PublicKeyModel>().ReverseMap();

            config.CreateMap<CompanyDto, CompanyModel>();
            config.CreateMap<CompanyPlanPointDto, CompanyPlanPointModel>();

            config.CreateMap<ManufactoryPlanPointDto, ManufactoryPlanPointModel>()
                  .ForMember(model => model.CompanyPlanPointId, cnf => cnf.MapFrom(dto => dto.CompanyPlanPointId));

            config.CreateMap<ManufactoryDto, ManufactoryModel>();
            config.CreateMap<CheckPointDto, CheckPointModel>();

            config.CreateMap<PipelineItemSettingsPrefabDto, PipelineItemSettingsPrefabModel>()
                  .ForPath(model => model.OptionDataType.Id, cnf => cnf.MapFrom(dto => dto.OptionDataTypeId));

            config.CreateMap<PipelineItemPrefabDto, PipelineItemPrefabModel>()
                  .ForMember(model => model.PipelineItemSettingsPrefabs, cnf => cnf.MapFrom(dto => dto.SettingsPrefabs));

            config.CreateMap<StorageCellPrefabDto, StorageCellPrefabModel>();

            config.CreateMap<DetectorDataPrefabDto, DetectorDataPrefabModel>()
                  .ForPath(model => model.FieldDataType.Id, cnf => cnf.MapFrom(dto => dto.FieldDataTypeId))
                  .ForPath(model => model.VisualizerType.Id, cnf => cnf.MapFrom(dto => dto.VisualizerTypeId));

            config.CreateMap<DetectorFaultPrefabDto, DetectorFaultPrefabModel>();

            config.CreateMap<DetectorSettingsPrefabDto, DetectorSettingsPrefabModel>()
                  .ForPath(model => model.OptionDataType.Id, cnf => cnf.MapFrom(dto => dto.OptionDataTypeId));

            config.CreateMap<DetectorPrefabDto, DetectorPrefabModel>();
        }
    }
}