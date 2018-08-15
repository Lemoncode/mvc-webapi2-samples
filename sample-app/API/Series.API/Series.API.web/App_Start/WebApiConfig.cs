using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Series.API.web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Return JSON format for text/html header, instead XML
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            // Web API configuration and services
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
