# Notes:

### Routing notes:

>Reference: https://docs.microsoft.com/es-es/aspnet/web-api/overview/getting-started-with-aspnet-web-api/tutorial-your-first-web-api
>Reference: https://docs.microsoft.com/en-us/aspnet/web-api/overview/web-api-routing-and-actions/routing-in-aspnet-web-api
>Reference: https://docs.microsoft.com/en-us/aspnet/web-api/overview/web-api-routing-and-actions/routing-and-action-selection
>Reference: https://docs.microsoft.com/en-us/aspnet/web-api/overview/web-api-routing-and-actions/attribute-routing-in-web-api-2



* To find the controller, Web API adds "Controller" to the value of the {controller} variable.

* To find the action, Web API looks at the HTTP method, and then looks for an action whose name begins with that HTTP method name. For example, with a GET request, Web API looks for an action that starts with "Get...", such as "GetContact" or "GetAllContacts". This convention applies only to GET, POST, PUT, and DELETE methods. You can enable other HTTP methods by using attributes on your controller. We'll see an example of that later.

* Other placeholder variables in the route template, such as {id}, are mapped to action parameters.


```C#
public class ProductsController : ApiController
{
    public IEnumerable<Product> GetAllProducts() { }
    public Product GetProductById(int id) { }
    public HttpResponseMessage DeleteProduct(int id){ }
}
```

HTTP Method		URI				Path			Action	Parameter
GET				api/products	GetAllProducts	(none)
GET				api/products/4	GetProductById	4
DELETE			api/products/4	DeleteProduct	4
POST			api/products	(no match)


### CORS notes

>Reference: https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/enabling-cross-origin-requests-in-web-api

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

* What if we wantto use it globally?

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

### Dependency Injection

>Reference: https://docs.microsoft.com/es-es/aspnet/web-api/overview/advanced/dependency-injection
>Reference: https://nodogmablog.bryanhogan.net/2016/04/web-api-2-and-ninject-how-to-make-them-work-together/