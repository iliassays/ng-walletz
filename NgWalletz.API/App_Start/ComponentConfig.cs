namespace NgWalletz.API
{
    using AspNet.Identity.MongoDB;
    using Autofac;
    using Autofac.Builder;
    using Autofac.Core;
    using Autofac.Integration.WebApi;
    using Entities;
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.Infrastructure;
    using Microsoft.Owin.Security.OAuth;
    using Owin;
    using Providers;
    using System;
    using System.Reflection;
    using System.Web.Http;
    

    public class ComponentConfig
    {
        public static IContainer Register()
        {
            var assemblies = new[] { Assembly.GetExecutingAssembly() };

            return RegisterWebApi(assemblies);
        }

        private static IContainer RegisterWebApi(Assembly[] assemblies)
        {
            var configuration = GlobalConfiguration.Configuration;
            var builder = CreateContainerBuilder();

            builder.RegisterWebApiFilterProvider(configuration);
            builder.RegisterWebApiModelBinders(assemblies);
            builder.RegisterApiControllers(assemblies);

            return builder.Build();
        }

        private static ContainerBuilder CreateContainerBuilder()
        {
            var builder = new ContainerBuilder();

            builder
                .RegisterType<MongoContext>()
                .AsImplementedInterfaces<MongoContext, ConcreteReflectionActivatorData>()
                .SingleInstance();

            builder
                .RegisterType<AuthRepository>()
                .SingleInstance();

            builder
                .RegisterType<ApplicationIdentityContext>()
                .SingleInstance();

            builder
                .RegisterType<UserStore<User>>()
                .AsImplementedInterfaces<IUserStore<User>, ConcreteReflectionActivatorData>()
                .SingleInstance();

            builder
                .RegisterType<RoleStore<Role>>()
                .AsImplementedInterfaces<IRoleStore<Role>, ConcreteReflectionActivatorData>()
                .SingleInstance();

            builder
                .RegisterType<ApplicationUserManager>()
                .SingleInstance();

            builder
                .RegisterType<ApplicationRoleManager>()
                .SingleInstance();

            builder
                .RegisterType<SimpleAuthorizationServerProvider>()
                .AsImplementedInterfaces<IOAuthAuthorizationServerProvider, ConcreteReflectionActivatorData>().SingleInstance();

            builder
                .RegisterType<SimpleRefreshTokenProvider>()
                .AsImplementedInterfaces<IAuthenticationTokenProvider, ConcreteReflectionActivatorData>().SingleInstance();

            return builder;
        }
    }
}
