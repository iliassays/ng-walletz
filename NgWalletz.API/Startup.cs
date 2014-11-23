[assembly: Microsoft.Owin.OwinStartup(typeof(NgWalletz.API.Startup))]

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
    using Microsoft.Owin.Cors;
    using Microsoft.Owin.Security.Facebook;
    using Microsoft.Owin.Security.Google;
    using Microsoft.Owin.Security.Infrastructure;
    using Microsoft.Owin.Security.OAuth;
    using Owin;
    using Providers;
    using System;
    using System.Configuration;
    using System.Threading.Tasks;
    using System.Web.Cors;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using WebApiResolver = Autofac.Integration.WebApi.AutofacWebApiDependencyResolver;

    public class Startup
    {
        public static OAuthBearerAuthenticationOptions oAuthBearerOptions { get; private set; }
        public static GoogleOAuth2AuthenticationOptions googleAuthOptions { get; private set; }
        public static FacebookAuthenticationOptions facebookAuthOptions { get; private set; }

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
            //use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);

            oAuthBearerOptions = new OAuthBearerAuthenticationOptions();

            var oAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = container.Resolve<IOAuthAuthorizationServerProvider>(),
                RefreshTokenProvider = container.Resolve<IAuthenticationTokenProvider>()
            };

            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(oAuthBearerOptions);

            googleAuthOptions = new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = ConfigurationManager.AppSettings["googleClientId"],
                ClientSecret = ConfigurationManager.AppSettings["googleClientSecret"],
                Provider = new GoogleAuthProvider()
            };

            app.UseGoogleAuthentication(googleAuthOptions);

            facebookAuthOptions = new FacebookAuthenticationOptions()
            {
                AppId = "xxxxxx",
                AppSecret = "xxxxxx",
                Provider = new FacebookAuthProvider()
            };
            app.UseFacebookAuthentication(facebookAuthOptions);

            
        }

        private void InitializeData(IContainer container)
        {
            var mongoContext = container.Resolve<IMongoContext>();

            if (mongoContext.Clients.Count() == 0)
            {
                mongoContext.Clients.Insert(new Client
                    {
                        Id = "NgWalletz",
                        Secret = Crypto.GetHash("shimulsays@gmail.com"),
                        Name = "AngularJS/WebAPI based Owin Authentication App",
                        ApplicationType = Models.ApplicationTypes.JavaScript,
                        Active = true,
                        RefreshTokenLifeTime = 7200,
                        AllowedOrigin = "http://localhost:32176"
                    });
            }
        }
    }
}