namespace MyWalletz.API
{
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;
    using System.Linq;
    using System.Net.Http.Formatting;
    using System.Web.Http;

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
