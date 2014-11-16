[assembly: Microsoft.Owin.OwinStartup(typeof(MyWalletz.API.Startup))]

namespace MyWalletz.API
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
    using System.Web.Http;
    using WebApiResolver = Autofac.Integration.WebApi.AutofacWebApiDependencyResolver;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var container = ComponentConfig.Register();
            app.UseAutofacMiddleware(container);

            var configuration = new HttpConfiguration
            {
                DependencyResolver = new WebApiResolver(container)
            };

            ConfigureOAuth(app, container);
            WebApiConfig.Register(configuration);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(configuration);
            app.UseAutofacWebApi(configuration);

            InitializeData(container);
        }

        private void ConfigureOAuth(IAppBuilder app, IContainer container)
        {
            var OAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = container.Resolve<IOAuthAuthorizationServerProvider>(),
                RefreshTokenProvider = container.Resolve<IAuthenticationTokenProvider>()
            };

            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        private void InitializeData(IContainer container)
        {
            var mongoContext = container.Resolve<IMongoContext>();

            if (mongoContext.Clients.Count() == 0)
            {
                mongoContext.Clients.Insert(new Client
                    {
                        Id = "myWalletz",
                        Secret = Crypto.GetHash("shimulsays@gmail.com"),
                        Name = "Angular JS based Owin Authentication App",
                        ApplicationType = Models.ApplicationTypes.JavaScript,
                        Active = true,
                        RefreshTokenLifeTime = 7200,
                        AllowedOrigin = "http://localhost"
                    });
            }
        }
    }
}