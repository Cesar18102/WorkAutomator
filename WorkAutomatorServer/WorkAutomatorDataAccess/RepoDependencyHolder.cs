using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Autofac;

using WorkAutomatorDataAccess.Entities;
using WorkAutomatorDataAccess.RepoInterfaces;
using WorkAutomatorDataAccess.Repos;
using WorkAutomatorDataAccess.Repos.PrefabRepos;

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

        public static IRepo<TEntity> ResolveRealRepo<TEntity>() where TEntity : EntityBase
        {
            return Dependencies.ResolveKeyed<IRepo<TEntity>>(ContextType.REAL);
        }

        private static List<Type> RegisteredEntityTypes = new List<Type>();

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
            RegisterRepo<DetectorPrefabRepo, DetectorPrefabEntity>(builder);
            RegisterRepo<PipelineItemPrefabRepo, PipelineItemPrefabEntity>(builder);

            Type[] entityTypes = Assembly.GetAssembly(typeof(EntityBase)).GetTypes().Where(
                type => typeof(EntityBase).IsAssignableFrom(type) && !type.IsAbstract && !RegisteredEntityTypes.Contains(type)
            ).ToArray();

            foreach(Type entityType in entityTypes)
            {
                MethodInfo registerBaseRepoGenericMethod = typeof(RepoDependencyHolder).GetRuntimeMethods().First(
                    m => m.Name == nameof(RegisterBaseRepoForEntityType)
                );

                registerBaseRepoGenericMethod.MakeGenericMethod(entityType).Invoke(null, new object[] { builder });
            }

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

            RegisteredEntityTypes.Add(typeof(TEntity));
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

            RegisteredEntityTypes.Add(typeof(TEntity));
        }
    }
}
