using System;

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

            RegisterRepo<AccountRepo, AccountEntity>(builder);
            RegisterRepo<CompanyRepo, CompanyEntity>(builder);
            RegisterRepo<StorageRepo, StorageCellEntity>(builder);
            RegisterRepo<ResourceRepo, ResourceEntity>(builder);
            RegisterRepo<ManufactoryRepo, ManufactoryEntity>(builder);
            RegisterRepo<RoleRepo, RoleEntity>(builder);
            RegisterRepo<PipelineRepo, PipelineEntity>(builder);
            RegisterRepo<PipelineItemRepo, PipelineItemEntity>(builder);
            RegisterRepo<DetectorRepo, DetectorEntity>(builder);

            return builder.Build();
        }

        private static void RegisterBaseRepoForEntityType<TEntity>(ContainerBuilder builder) where TEntity : EntityBase
        {
            builder.RegisterType<RepoBase<TEntity>>()
                   .As<IRepo<TEntity>>()
                   .Keyed<IRepo<TEntity>>(ContextType.REAL)
                   .UsingConstructor(new Type[] { typeof(Type) })
                   .WithParameter(new TypedParameter(typeof(Type), typeof(WorkAutomatorDBContext)))
                   .SingleInstance();

            builder.RegisterType<RepoBase<TEntity>>()
                   .As<IRepo<TEntity>>()
                   .Keyed<IRepo<TEntity>>(ContextType.TEST)
                   .UsingConstructor(new Type[] { typeof(Type) })
                   .WithParameter(new TypedParameter(typeof(Type), typeof(WorkAutomatorDBContextTest)))
                   .SingleInstance();
        }

        private static void RegisterRepo<TRepo, TEntity>(ContainerBuilder builder)
            where TEntity : EntityBase
            where TRepo : IRepo<TEntity>
        {
            builder.RegisterType<TRepo>()
                   .As<RepoBase<TEntity>>()
                   .As<IRepo<TEntity>>()
                   .AsSelf()
                   .Keyed<IRepo<TEntity>>(ContextType.REAL)
                   .UsingConstructor(new Type[] { typeof(Type) })
                   .WithParameter(new TypedParameter(typeof(Type), typeof(WorkAutomatorDBContext)))
                   .SingleInstance();

            builder.RegisterType<TRepo>()
                   .As<RepoBase<TEntity>>()
                   .As<IRepo<TEntity>>()
                   .AsSelf()
                   .Keyed<IRepo<TEntity>>(ContextType.TEST)
                   .UsingConstructor(new Type[] { typeof(Type) })
                   .WithParameter(new TypedParameter(typeof(Type), typeof(WorkAutomatorDBContextTest)))
                   .SingleInstance();
        }
    }
}
