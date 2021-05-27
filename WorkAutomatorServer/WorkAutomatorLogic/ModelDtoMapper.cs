using AutoMapper;

using Dto;

using WorkAutomatorLogic.Models;

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
                  .ForMember(model => model.CompanyPlanPointId, cnf => cnf.MapFrom(dto => dto.Id));

            config.CreateMap<ManufactoryDto, ManufactoryModel>();
        }
    }
}