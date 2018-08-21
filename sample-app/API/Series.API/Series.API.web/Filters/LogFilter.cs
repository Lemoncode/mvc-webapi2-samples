using Ninject.Web.WebApi.Filter;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Series.API.web.Filters
{
    public class LogFilter : AbstractActionFilter
    {
        private readonly ILogger log;

        public LogFilter(ILogger log)
        {
            this.log = log;
        }

        public override bool AllowMultiple => true;

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            log.Info("LogFilter action executing");
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            log.Info("LogFilter action executed");
        }
    }
}