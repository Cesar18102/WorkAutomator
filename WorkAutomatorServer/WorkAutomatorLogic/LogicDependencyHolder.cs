using Autofac;

using WorkAutomatorLogic.ServiceInterfaces;
using WorkAutomatorLogic.Services;

namespace WorkAutomatorLogic
{
    public static class LogicDependencyHolder
    {
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

            builder.RegisterType<InitService>().As<IInitService>().SingleInstance();

            builder.RegisterType<KeyService>().As<IKeyService>().SingleInstance();
            builder.RegisterType<RSAService>().As<IAsymmetricEncryptionService>().SingleInstance();
            builder.RegisterType<SHA256HashingService>().As<IHashingService>().SingleInstance();

            builder.RegisterType<AuthService>().As<IAuthService>().SingleInstance();
            builder.RegisterType<SessionService>().As<ISessionService>().SingleInstance();
            builder.RegisterType<PermissionService>().As<IPermissionService>().SingleInstance();
            builder.RegisterType<CompanyService>().As<ICompanyService>().SingleInstance();
            builder.RegisterType<LocationService>().As<ILocationService>().SingleInstance();
            builder.RegisterType<RoleService>().As<IRoleService>().AsSelf().SingleInstance();
            builder.RegisterType<PrefabService>().As<IPrefabService>().SingleInstance();
            builder.RegisterType<PipelineItemService>().As<IPipelineItemService>().SingleInstance();
            builder.RegisterType<StorageCellService>().As<IStorageCellService>().SingleInstance();
            builder.RegisterType<DetectorService>().As<IDetectorService>().SingleInstance();
            builder.RegisterType<PipelineService>().As<IPipelineService>().SingleInstance();
            builder.RegisterType<IntersectionService>().AsSelf().SingleInstance();
            builder.RegisterType<DataTypeService>().AsSelf().SingleInstance();
            builder.RegisterType<FaultConditionParseService>().AsSelf().SingleInstance();
            builder.RegisterType<AccountService>().As<IAccountService>().SingleInstance();
            builder.RegisterType<TaskService>().As<ITaskService>().SingleInstance();

            return builder.Build();
        }
    }
}
