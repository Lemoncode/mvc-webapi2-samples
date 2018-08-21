[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Series.API.web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Series.API.web.App_Start.NinjectWebCommon), "Stop")]

namespace Series.API.web.App_Start
{
    using System;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Filters;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    using Ninject.Web.WebApi;
    using Ninject.Web.WebApi.FilterBindingSyntax;
    using Security.Utils;
    using Security.Validators;
    using Series.API.web.Filters;
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
            kernel.Bind<IContainerRepositories>()
                .To<ContainerRepositories>()
                .WithConstructorArgument("connectionString", "");

            kernel.Bind<ISecurityRepository>()
                .To<SecurityRepository>();

            kernel.Bind<IHashGenerator>()
                .To<HashGenerator>();

            kernel.Bind<IContentValidator>()
                .To<HashValidator>();

            kernel.Bind<ITokenGenerator>()
                .To<TokenGenerator>();

            kernel.Bind<ILogger>().To<Logger>();
            kernel.BindHttpFilter<LogFilter>(System.Web.Http.Filters.FilterScope.Controller);

            kernel.BindHttpFilter(
                    x => new ValidateUserFilter(
                        x.Inject<ISecurityRepository>(),
                        x.FromActionAttribute<ValidateUserAttribute>().GetValue(att => att.UserType)),
                    FilterScope.Action
                ).WhenActionMethodHas<ValidateUserAttribute>();
        }        
    }
}