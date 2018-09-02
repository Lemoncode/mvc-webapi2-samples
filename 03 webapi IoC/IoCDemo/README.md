# In this demo we are going to create an IoC container that will manage our dependencies.

* We will use Ninject, this will provide us with an IoC container.
* We will set up the Web API environment to use it.

## Steps.

### 1. First of all we will create an ecosystem of external libraries.

* At the root level of `.sln` folder create a new folder `libs.bin`

### 2. We will create a new service to calculate the area of geometrical shapes.

* Create a new solution `Shapes`
* Create a new library project `Shapes.Contracts` 
* Create a new interface `IAreaResolver`

```C#
namespace Shapes.Contracts
{
    public interface IAreaResolver
    {
        double CircleArea(double radius);
    }
}
```
* Create a new library project Shapes.Services
* Create a new class AreaResolver this class will implement IAreaResolver
	- We have to add the reference to the contracts project

```C#
using Shapes.Contracts;
using System;

namespace Shapes.Services
{
    public class AreaResolver : IAreaResolver
    {
        public double CircleArea(double radius)
        {
            return Math.PI * Math.Pow(radius, 2);
        }
    }
}

```
* Remind that we have to point out our build to `libs.bin`. Use relative path.

### 3. Now we can reference these libs in our Web API project.

* Add a reference to Shape.Contracts
* Add a reference to Shape.Services

### 4. It's time to start to use Ninject in our solution to resolve our dependencies

* Add a new reference to the following package:
	- Ninject.Web.WebApi
* Adding just this package, we don't have to care about other Ninject dependencies, it will install for us:
	- Ninject
	- Ninject.Web.Common
	- Ninject.Web.WebApi

* To make our live easier, and forgive about to create by ourselve the plumbing we can use another package:
	- Ninject.Web.Common.WebHost
* Adding this package will get some extra dependencies:
	- Microsoft.Web.Infraestructure.1.0.0
	- WebActivatorEx.2.2.0
	- Ninject.Web.Common.WebHost.3.3.1

*  This will pull down the WebActivatorEx package and add a new class called Ninject.Web.Common.cs to your App_Start directory.

### 5. If we open NinjectWebCommon, we will find out that there are missing assemblies. Let's solve that.

```diff Ninject.Web.Common.cs
[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(IoCDemo.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(IoCDemo.App_Start.NinjectWebCommon), "Stop")]

namespace IoCDemo.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
+   using Ninject.Web.Common;
+   using Ninject.Web.Common.WebHost;

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

### 6. With all this on place we have to add a line of code and make Ninject the dependency resolver for Web API.

```diff Ninject.Web.Common.cs
[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(IoCDemo.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(IoCDemo.App_Start.NinjectWebCommon), "Stop")]

namespace IoCDemo.App_Start
{
    using System;
+   using System.Web;
+   using System.Web.Http;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
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

### 7. Now we can register a service as follows.

```diff
[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(IoCDemo.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(IoCDemo.App_Start.NinjectWebCommon), "Stop")]

namespace IoCDemo.App_Start
{
    using System;
    using System.Web;
    using System.Web.Http;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    using Ninject.Web.WebApi;
    using Shapes.Contracts;
    using Shapes.Services;

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
+           kernel.Bind<IAreaResolver>().To<AreaResolver>();
        }        
    }
}
```

### 8. Now we can use it in our controllers.

* Create a new controller `AreaShapeController.cs`

```C#
using Shapes.Contracts;
using System.Web.Http;

namespace IoCDemo.Controllers
{
    public class AreShapeController : ApiController
    {
        private IAreaResolver _areaResolverService;

        public AreShapeController(IAreaResolver areaResolverService)
        {
            _areaResolverService = areaResolverService;
        }

        [Route("shapes/circle/{radius}/area")]
		[HttpGet]
        public IHttpActionResult CircleArea(string radius)
        {
            double _radius;
            if (double.TryParse(radius, out _radius))
            {
                return Ok(_areaResolverService.CircleArea(_radius));
            }

            return BadRequest($"Not valid radius: {radius}");
        }
    }
}

```

* Now we have to test our controller and see if it's working