namespace NgWalletz.API
{
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;
    using System.Linq;
    using System.Net.Http.Formatting;
    using System.Runtime.Remoting.Contexts;
    using System.Web.Http;
    using System.Web.Http.Cors;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
            var jsonSettings = configuration.Formatters
                .JsonFormatter
                .SerializerSettings;

            jsonSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();

            jsonSettings.Converters.Add(new StringEnumConverter());

            configuration.MapHttpAttributeRoutes();

            configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
