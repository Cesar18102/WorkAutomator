using Autofac;
using System.Threading.Tasks;
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

            return builder.Build();
        }
    }
}
