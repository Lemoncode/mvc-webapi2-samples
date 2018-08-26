# In this demo we are going to explore the routing on MVC Web API 2

>NOTE: Start a new empty web project and select Web API option for scaffolding.

## 1. Related concepts.

* In ASP.NET Web API, a controller is a class that handles HTTP requests. 
* The public methods of the controller are called action methods or simply actions. 
* When the Web API framework receives a request, it routes the request to an action.
* To determine which action to invoke, the framework uses a routing table.

```C# WebApiConfig.cs
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

            config.Routes.MapHttpRoute( // [1]
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}

```
1. Each entry in the routing table contains a route template. 
	- The default route template for Web API is "api/{controller}/{id}". 
	- In this template, "api" is a literal path segment, and {controller} and {id} are placeholder variables.

* When the Web API framework receives an HTTP request, it tries to match the URI against one of the route templates in the routing table. 
* If no route matches, the client receives a 404 error. 

* For example, the following URIs match the default route:
	- /api/contacts
	- /api/contacts/1
	- /api/products/gizmo1
* Not much:
	- /contacts/1

* When you get a matching route:
	- To find the controller, Web API adds "Controller" to the value of the {controller} variable.
	- To find the action, Web API looks at the HTTP method, and then looks for an action whose name begins with that HTTP method name.
		- For example, with a GET request, Web API looks for an action that starts with "Get...", such as "GetContact" or "GetAllContacts".
		- This convention applies only to GET, POST, PUT, and DELETE methods.
		- You can enable other HTTP methods by using attributes on your controller
	- Other placeholder variables in the route template, such as {id}, are mapped to action parameters.

## 2. Let's see a code example of this:

* Create a new web api empty controller, `SeriesController.cs`.
* Create a serie model inside Models folder, `Serie.cs`

```C#
namespace RoutingDemo.Models
{
    public class Serie
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
```

```C# SeriesController.cs
using RoutingDemo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RoutingDemo.Controllers
{
    public class SeriesController : ApiController
    {
        private static List<Serie> _series = new List<Serie>
            {
                new Serie
                {
                    Id = 1,
                    Title = "A",
                },
                new Serie
                {
                    Id = 2,
                    Title = "B",
                },
            };

        public IEnumerable<Serie> GetSeries()
        {
            return _series;
        }

        public Serie GetSerieById(int id)
        {
            return _series.SingleOrDefault(s => s.Id == id);
        }

        public HttpResponseMessage DeleteSerie(int id)
        {
            var serieToRemove = _series.SingleOrDefault(s => s.Id == id);

            if (serieToRemove == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            else
            {
                var index = _series.IndexOf(serieToRemove);
                _series.RemoveAt(index);
                return new HttpResponseMessage(HttpStatusCode.Accepted);
            }
        }
    }
}

```
* Let's run our application and test the created controller actions

HTTP Method		URI				Path			Action	Parameter
GET				api/series	    GetSeies		(none)
GET				api/series/1	GetSerieById	1
DELETE			api/series/1	DeleteSerie		1
POST			api/sereies  	(no match)

### 3. Routing variations. HTTP Methods.

* Instead of using the naming convention for HTTP methods, you can explicitly specify the HTTP method for an action by decorating the action method with the HttpGet, HttpPut, HttpPost, or HttpDelete attribute.
* Let's demonstrate this by adding a new file to `Models` folder, `Type.cs`

```C# Type.cs
namespace RoutingDemo.Models
{
    public class Type
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
}
```
* Modify `Serie.cs` as follows:

```diff Serie.cs
namespace RoutingDemo.Models
{
    public class Serie
    {
        public int Id { get; set; }
        public string Title { get; set; }
+       public Type Type { get; set; }
    }
}
```

* And for last changes our mocked data:

```diff SeriesController.cs
private static List<Serie> _series = new List<Serie>
            {
                new Serie
                {
                    Id = 1,
                    Title = "A",
+                   Type = new Type { Id = 1, Description = "Drama" },
                },
                new Serie
                {
                    Id = 2,
                    Title = "B",
+                   Type = new Type { Id = 2, Description = "Action" }
                },
            };
```

* Now we are ready to write something like this:

```C# SeriesController.cs
// http://localhost:53135/api/series/findseriesbytype?typeId=1
[HttpGet]
public IEnumerable<Serie> FindSeriesByType(int typeId)
{
    return _series
        .Where(s => s.Type.Id == typeId);
}
```
* In order to get this working we have to use query params.
* If we run this now we will get the following error:

```text
Multiple actions were found that match the request: GetSerieById on type RoutingDemo.Controllers.SeriesController FindSeriesByType on type RoutingDemo.Controllers.SeriesController
```

* If we comment out `GetSerieById` method, this get to work. 
* What is already happening here is that web api can't figure out how to resolve this.

* Let's try another approach:

```diff WebApiConfig.cs
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

-			config.Routes.MapHttpRoute(
-			    name: "DefaultApi",
-			    routeTemplate: "api/{controller}/{id}",
-			    defaults: new { id = RouteParameter.Optional }
-			);
+            config.Routes.MapHttpRoute(
+                name: "ControllerPlusId",
+                routeTemplate: "api/{controller}/{id}",
+                defaults: null,
+                constraints: new { id = @"^\d+$" }
+            );
+
+            config.Routes.MapHttpRoute(
+                name: "CustomActionPlusId",
+                routeTemplate: "api/{controller}/{action}/{id}",
+                defaults: null,
+                constraints: new { id = @"^\d+$" }
+            );
+
+            config.Routes.MapHttpRoute(
+                name: "CustomAction",
+                routeTemplate: "api/{controller}/{action}"
+            );
+
+
+
+            config.Routes.MapHttpRoute(
+                name: "OnlyController",
+                routeTemplate: "api/{controller}",
+                defaults: new { action = "" }       // If we dont define this defaults, webapi confuse between custom action and default actions.
+            );
        }
    }
}

```

* In order to make this work we have to modify our controller:

```diff SeriesController
using RoutingDemo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RoutingDemo.Controllers
{
    public class SeriesController : ApiController
    {
        private static List<Serie> _series = new List<Serie>
            {
                new Serie
                {
                    Id = 1,
                    Title = "A",
                    Type = new Type { Id = 1, Description = "Drama" },
                },
                new Serie
                {
                    Id = 2,
                    Title = "B",
                    Type = new Type { Id = 2, Description = "Action" }
                },
            };

        // http://localhost:53135/api/series
+        [ActionName("")]
        public IEnumerable<Serie> GetSeries()
        {
            return _series;
        }

        // http://localhost:53135/api/series/1
+        [ActionName("")]
        public Serie GetSerieById(int id)
        {
            return _series.SingleOrDefault(s => s.Id == id);
        }

        // http://localhost:53135/api/series/2
+        [ActionName("")]
        public HttpResponseMessage DeleteSerie(int id)
        {
            var serieToRemove = _series.SingleOrDefault(s => s.Id == id);

            if (serieToRemove == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            else
            {
                var index = _series.IndexOf(serieToRemove);
                _series.RemoveAt(index);
                return new HttpResponseMessage(HttpStatusCode.Accepted);
            }
        }

        // http://localhost:53135/api/series/findseriesbytype?typeId=1
        [HttpGet]
        public IEnumerable<Serie> FindSeriesByType(int typeId)
        {
            return _series
                .Where(s => s.Type.Id == typeId);
        }
+
+		// http://localhost:53135/api/series/findbytitle?title=A
+        [HttpGet]
+        public Serie FindByTitle(string title)
+        {
+            return _series.SingleOrDefault(s => s.Title == title);
+        }
    }
}

```
* For last we have added a new method with a different type of digit.
* If we want to pass any type of parameter  to our actions we have to modify the regex.