using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Routing;

namespace CustomFilter.Demo.Filters
{
    public class Log : ActionFilterAttribute
    {
        public override bool AllowMultiple => true; // [1]

        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var routeData = actionContext.ControllerContext.RouteData;
            return Logger("OnActionExecuting", routeData);
        }

        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            var routeData = actionExecutedContext.ActionContext.ControllerContext.RouteData;
            return Logger("OnActionExecuted", routeData);
        }

        private Task Logger(string method, IHttpRouteData routeData)
        {
            return Task.Factory // [2]
                .StartNew
                (
                    () => 
                    {
                        var controller = routeData.Values["controller"];
                        var action = routeData.Values["action"];
                        var message = $"{method} controller: {controller} action: {action}";
                        Debug.WriteLine(message, "Action filter Log");
                    }
                );
        }
    }
}