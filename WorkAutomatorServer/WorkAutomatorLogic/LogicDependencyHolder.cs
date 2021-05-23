﻿using System.Security.Cryptography;

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

            builder.RegisterType<KeyService>().As<IKeyService>().SingleInstance();
            builder.RegisterType<RSAService>().As<IAsymmetricEncryptionService>().SingleInstance();
            builder.RegisterType<SHA256HashingService>().As<IHashingService>().SingleInstance();

            builder.RegisterType<AuthService>().As<IAuthService>().SingleInstance();
            builder.RegisterType<SessionService>().As<ISessionService>().SingleInstance();

            return builder.Build();
        }
    }
}
