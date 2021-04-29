using AutoMapper;

using WorkAutomatorDataAccess.Entities;
using WorkAutomatorLogic.Models;

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
            //config.CreateMap<PublicKeyDto, PublicKeyModel>().ReverseMap();
            //config.CreateMap<SignUpDto, SignUpFormModel>();
            //config.CreateMap<LogInDto, LogInFormModel>();
        }
    }
}
