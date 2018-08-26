using System.Web.Http;

namespace RoutingDemo
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            config.Routes.MapHttpRoute(
                name: "ControllerPlusId",
                routeTemplate: "api/{controller}/{id}",
                defaults: null,
                constraints: new { id = @"^\d+$" }
            );

            config.Routes.MapHttpRoute(
                name: "CustomActionPlusId",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: null,
                constraints: new { id = @"^\d+$" }
            );

            config.Routes.MapHttpRoute(
                name: "CustomAction",
                routeTemplate: "api/{controller}/{action}"
            );

            config.Routes.MapHttpRoute(
                name: "OnlyController",
                routeTemplate: "api/{controller}",
                defaults: new { action = "" }       // If we dont define this defaults, webapi confuse between custom action and default actions.
            );
        }
    }
}
