using Autofac;

using WorkAutomatorDataAccess.Entities;
using WorkAutomatorDataAccess.RepoInterfaces;
using WorkAutomatorDataAccess.Repos;

namespace WorkAutomatorDataAccess
{
    public static class RepoDependencyHolder
    {
        public enum ContextType
        {
            REAL,
            TEST
        }

        private static IContainer denendencies = null;

        public static IContainer Dependencies
        {
            get
            {
                if (denendencies == null)
                    denendencies = BuildDependencies();
                return denendencies;
            }
        }

        private static IContainer BuildDependencies()
        {
            ContainerBuilder builder = new ContainerBuilder();

            RegisterBaseRepoForEntityType<AccountEntity>(builder);

            return builder.Build();
        }

        private static void RegisterBaseRepoForEntityType<TEntity>(ContainerBuilder builder) where TEntity : EntityBase
        {
            builder.RegisterType<RepoBase<WorkAutomatorDBContext, TEntity>>()
                   .As<IRepo<TEntity>>()
                   .Keyed<IRepo<TEntity>>(ContextType.REAL)
                   .SingleInstance();

            builder.RegisterType<RepoBase<WorkAutomatorDBContextTest, TEntity>>()
                   .As<IRepo<TEntity>>()
                   .Keyed<IRepo<TEntity>>(ContextType.TEST)
                   .SingleInstance();
        }
    }
}
