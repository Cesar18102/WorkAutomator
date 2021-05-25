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

        public static UserActionModel<TModel> ToModel<TDto, TModel>(this AuthorizedDto<TDto> dto)
            where TDto : IdDto
            where TModel : ModelBase
        {
            UserActionModel<TModel> model = new UserActionModel<TModel>();

            model.UserAccountId = dto.Session.UserId;
            model.Data = Mapper.Map<TModel>(dto.Data);

            return model;
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
            config.CreateMap<SessionDto, SessionCredentialsModel>();

            config.CreateMap<CompanyDto, CompanyModel>();
        }
    }
}