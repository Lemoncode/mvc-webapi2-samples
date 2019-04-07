# In this demo we are going to cover more advanced DI scenarios.

## Steps

### 1. Create a new Web API project

* `DIAdvancedScenarios.API`

* Empty 
* Check for Web API 

### 2. Lets create a custom Log filter.

* We will use `ActionFilterAttribute` as base class to create our filter.

* Create a new folder `Filters`

* I want to DI with this filter. Loose coupling and testable code it's our goal.

* Inside this folder place the following interface:

```C#
namespace DIAdvancedScenarios.API.Filters
{
    public interface ILogger
    {
        void Info(string message);
        void Warning(string message);
    }
}

```

* Lets create as well a service that implement this service, inside `Filters`, just for simplicity

```C#
using System.Diagnostics;

namespace DIAdvancedScenarios.API.Filters
{
    public class LoggerService : ILogger
    {
        public void Info(string message)
        {
            Debug.WriteLine($"Information: {message}");
        }

        public void Warning(string message)
        {
            Debug.WriteLine($"Warning: {message}");
        }
    }
}
```

* Ok then, with this on place we can start trto create our filter. Place this code inside `Filters` folder:

```C#
using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace DIAdvancedScenarios.API.Filters
{
    public class LoggerAttribute : ActionFilterAttribute
    {
        private ILogger _logger;

        public LoggerAttribute(ILogger logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            _logger.Info("Processing action");
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var objectContent = actionExecutedContext.Response.Content as ObjectContent;
            Type type = null;
            object value = null;

            if (objectContent != null)
            {
                type = objectContent.ObjectType;
                value = objectContent.Value;
            }

            _logger.Info
                (
                    $"Response status code: {actionExecutedContext.Response.StatusCode.ToString()} " +
                    $"type: {type.ToString()}" +
                    $" value: {value.ToString()}"
                );
        }
    }
}
```

### 3. Now lets create a new controller and verify that we can reach it.

* Add a new empty Web API 2 controller.

```C#
using System.Collections.Generic;
using System.Web.Http;

namespace DIAdvancedScenarios.API.Controllers
{
    public class ComicsController : ApiController
    {
        // api/comics
        public IEnumerable<string> GetTitles()
        {
            return new string[] { "Batman", "Superman" };
        }
    }
}

```

* Test that we can hit the controller and we obtain results.

### 4. Now it's time to try to use the filter that we already have created.

```C#
using DIAdvancedScenarios.API.Filters;
using System.Collections.Generic;
using System.Web.Http;

namespace DIAdvancedScenarios.API.Controllers
{
    public class ComicsController : ApiController
    {
        // api/comics
        /*diff*/
        [Logger(new LoggerService())]
        /*diff*/
        public IEnumerable<string> GetTitles()
        {
            return new string[] { "Batman", "Superman" };
        }
    }
}

```

* If we compile the project we get the following build error: `Error	CS0181	Attribute constructor parameter 'logger' has type 'ILogger', which is not a valid attribute parameter type`.

* That is I can not instance a new type and passing as argument in constructor, I can just pass literals that doesn't imply `new`.

### 5. Ninject to the rescue.

* Install the following packages:
    - Ninject.Web.WebApi
    - Ninject.Web.Common
    - Ninject.Web.Common.WebHost

* Modify `Ninject.Web.Common.cs` as follows:

```diff
[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(DIAdvancedScenarios.API.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(DIAdvancedScenarios.API.App_Start.NinjectWebCommon), "Stop")]

namespace DIAdvancedScenarios.API.App_Start
{
    using System;
+   using System.Web;
+   using System.Web.Http;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
+   using Ninject.Web.Common;
+   using Ninject.Web.Common.WebHost;
+   using Ninject.Web.WebApi;

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
+               GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
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
        }        
    }
}
```

### 6. Ok. with this minimum configuration, we are going to create a new Filter.

* Place this code in `Filters` folder:

```C#
using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace DIAdvancedScenarios.API.Filters
{
    public class LoggerFilter : ActionFilterAttribute
    {
        private ILogger _logger;

        public LoggerFilter(ILogger logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            _logger.Info("Processing action");
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var objectContent = actionExecutedContext.Response.Content as ObjectContent;
            Type type = null;
            object value = null;

            if (objectContent != null)
            {
                type = objectContent.ObjectType;
                value = objectContent.Value;
            }

            _logger.Info
                (
                    $"Response status code: {actionExecutedContext.Response.StatusCode.ToString()} " +
                    $"type: {type.ToString()}" +
                    $" value: {value.ToString()}"
                );
        }
    }
}
```

* Is the same code but just renaming to `LoggerFilter`

### 7. Now lets add some changes to our logger attribute

* Modify as follows the `LoggerAttribute`

```diff
using System.Web.Http.Filters;

namespace DIAdvancedScenarios.API.Filters
{
-   public class LoggerAttribute : ActionFilterAttribute
    public class LoggerAttribute : FilterAttribute
    {
-       private ILogger _logger;

-       public LoggerAttribute(ILogger logger)
-       {
-           _logger = logger;
-       }

-       public override void OnActionExecuting(HttpActionContext actionContext)
-       {
-           _logger.Info("Processing action");
-       }

-       public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
-       {
-           var objectContent = actionExecutedContext.Response.Content as ObjectContent;
-           Type type = null;
-           object value = null;

-           if (objectContent != null)
-           {
-               type = objectContent.ObjectType;
-               value = objectContent.Value;
-           }

-           _logger.Info
-               (
-                   $"Response status code: {actionExecutedContext.Response.StatusCode.ToString()} " +
-                   $"type: {type.ToString()}" +
-                   $" value: {value.ToString()}"
-               );
-       }
    }
}
```

### 8. Ok, almost there is time to configure Ninject.

```C#
[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(DIAdvancedScenarios.API.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(DIAdvancedScenarios.API.App_Start.NinjectWebCommon), "Stop")]

namespace DIAdvancedScenarios.API.App_Start
{
    using System;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Filters;
    using DIAdvancedScenarios.API.Filters;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    using Ninject.Web.WebApi;
    using Ninject.Web.WebApi.FilterBindingSyntax;

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
                GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
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
            /*diff*/
            kernel.Bind<ILogger>()
                .To<LoggerService>();

            kernel.BindHttpFilter(
                    x => new LoggerFilter(
                            x.Inject<ILogger>()),
                            FilterScope.Action
                        ).WhenActionMethodHas<LoggerAttribute>();
            /*diff*/
        }        
    }
}
```

### 9. Now in our controller we can define as follows.

```diff
using DIAdvancedScenarios.API.Filters;
using System.Collections.Generic;
using System.Web.Http;

namespace DIAdvancedScenarios.API.Controllers
{
    public class ComicsController : ApiController
    {
        // api/comics
-       [Logger(new LoggerService())]
+       [Logger]
        public IEnumerable<string> GetTitles()
        {
            return new string[] { "Batman", "Superman" };
        }
    }
}

```

###  10. Lets test if every goes find. Put breakpoints into LoggerFilter