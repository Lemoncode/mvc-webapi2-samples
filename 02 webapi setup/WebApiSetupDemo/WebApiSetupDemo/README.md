# In this demo we are going to setup some features of Web API

## 1. Enable CORS

* Install-Package Microsoft.AspNet.WebApi.Cors

```C# WebApiConfig.cs
using System.Web.Http;
namespace WebService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // New code
            config.EnableCors();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
```

* To use in our controllers

```C# {controller}.cs
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebService.Controllers
{
    [EnableCors(origins: "http://mywebclient.azurewebsites.net", headers: "*", methods: "*")]
    public class TestController : ApiController
    {
        // Controller methods not shown...
    }
}
```
* We can use the tag by action inside of the controller

* What if we want to use it globally?

```C# WebApiConfig.cs
public static class WebApiConfig
{
    public static void Register(HttpConfiguration config)
    {
        var cors = new EnableCorsAttribute("www.example.com", "*", "*");
        config.EnableCors(cors);
        // ...
    }
}
```

* To test that our configuration is working, use ~\02 webapi setup\WebApiClient, vanilla parcel javascript project to test this functionality.

## 2. At the moment maybe we do not notice that we are returning XML instead JSON, let's change that:

```diff WebApiConfig.cs
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Series.API.web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
+           // Return JSON format for text/html header, instead XML
+           config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
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

```

* Now if we make a request agianst a browser, we have to notice the format change.

## 3. One last step to get our set up ready, the JSON format it's in Pascal case instead Camel case, to demonstrate this. let's add a new class in Models folder:

```C# Serie.cs
namespace WebApiSetupDemo.Models
{
    public class Serie
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
```

* In our controller, we create a new method

```diff SeriesController.cs
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApiSetupDemo.Models;

namespace WebApiSetupDemo.Controllers
{
    public class SeriesController : ApiController
    {
        static List<string> _titles = new List<string> { "A", "B" };

        // http://localhost:65423/api/series
        public IEnumerable<string> GetSeriesTitles()
        {
            return _titles;
        }

+		// http://localhost:65423/api/series/complete
+      [Route("api/series/complete")]
+      public IEnumerable<Serie> GetSeries()
+      {
+          return new List<Serie> { new Serie { Id = 1, Title = "A" } };
+      } 
    }
}

```

* Let's test that the returned format is pascal instead camel

## 4. To fix this issue, we have to modify Global.asax.cs

```diff Global.asax.cs
using System.Web.Http;

namespace WebApiSetupDemo
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
+			GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}

```