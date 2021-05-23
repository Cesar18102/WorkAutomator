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
            config.CreateMap<SignUpFormModel, AccountEntity>()
                  .ForMember(entity => entity.password, cnf => cnf.MapFrom(model => model.PasswordEncrypted));

            config.CreateMap<AccountModel, AccountEntity>()
                  .ForMember(entity => entity.first_name, cnf => cnf.MapFrom(model => model.FirstName))
                  .ForMember(entity => entity.last_name, cnf => cnf.MapFrom(model => model.LastName))
                  .ReverseMap()
                  .ForMember(model => model.FirstName, cnf => cnf.MapFrom(entity => entity.first_name))
                  .ForMember(model => model.LastName, cnf => cnf.MapFrom(entity => entity.last_name));
        }
    }
}
