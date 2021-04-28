using AutoMapper;

using WorkAutomatorLogic.Models;
using WorkAutomatorServer.Dto;

namespace WorkAutomatorServer
{
    public static class DtoModelMapper
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
            config.CreateMap<SignUpDto, SignUpFormModel>();
            config.CreateMap<LogInDto, LogInFormModel>();
        }
    }
}