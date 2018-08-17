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

* Web API 2 supports a new type of routing, called attribute routing. As the name implies, attribute routing uses attributes to define routes. Attribute routing gives you more control over the URIs in your web API. For example, you can easily create URIs that describe hierarchies of resources.

* The earlier style of routing (convention-based) is fully support

* Why Attribute routing?
	- One advantage of convention-based routing is that templates are defined in a single place, and the routing rules are applied consistently across all controllers. Unfortunately, convention-based routing makes it hard to support certain URI patterns that are common in RESTful APIs. For example, resources often contain child resources: Customers have orders, movies have actors, books have authors, and so forth. It's natural to create URIs that reflect these relations:
		- /customers/1/orders

```C#
[Route("customers/{customerId}/orders")]
public IEnumerable<Order> GetOrdersByCustomer(int customerId) { ... }
```

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

* We have used the second link.
* Because we are using IContainerRepositories, to make the hold thing work we have to get the instances with arguments.

```C# Ninject.Web.Common.cs
[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Series.API.web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Series.API.web.App_Start.NinjectWebCommon), "Stop")]

namespace Series.API.web.App_Start
{
    using System;
    using System.Web;
    using System.Web.Http;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    using Ninject.Web.WebApi;
    using Series.Backend.Contracts;
    using Series.Backend.Models;
    using Series.Backend.Models.Repositories;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                RegisterServices(kernel);
                GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel); // [1]
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IContainerRepositories>()
                .To<ContainerRepositories>()
                .WithConstructorArgument("connectionString", ""); // [2]
        }        
    }
}
```

1. We have to register it at `GlobalConfiguration` otherwise we got an error. Bear on mind that if we want to pass a connection string we can do at the Web.config file.
2. Because we do not have default constructor, we have to invoke the constructor with arguments.

* For filtering injection have a look at: 
	- https://stackoverflow.com/questions/6193414/dependency-injection-with-ninject-and-filter-attribute-for-asp-net-mvc

### Return JSON format from our API.

>Reference: https://stackoverflow.com/questions/9847564/how-do-i-get-asp-net-web-api-to-return-json-instead-of-xml-using-chrome/20556625#20556625
>Reference: https://stackoverflow.com/questions/28552567/web-api-2-how-to-return-json-with-camelcased-property-names-on-objects-and-the

* Here we got different goals:
	1. Return JSON format instead XML for some kind of requests.
	2. Return camel case format instead of Pascal case.

1. Return JSON format instead XML for some kind of requests.

```C# WebApiConfig.cs
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

```

2. Return camel case format instead of Pascal case.

```C# Global.asax.cs
using Newtonsoft.Json.Serialization;
using System.Web.Http;

namespace Series.API.web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Convert to camel case JSON response 
            // https://stackoverflow.com/questions/28552567/web-api-2-how-to-return-json-with-camelcased-property-names-on-objects-and-the
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}

```

### Unit Testing

* Install Moq
* Create isolated example