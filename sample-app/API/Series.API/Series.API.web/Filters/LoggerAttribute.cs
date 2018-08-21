using Ninject;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Series.API.web.Filters
{
    public class LoggerAttribute : ActionFilterAttribute
    {
        [Inject]
        public ILogger Logger { get; set; }
        public LoggerAttribute()
        {
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            Logger.Info("Pre-processing logging");
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var objectContent = actionExecutedContext.Response.Content as ObjectContent;
            if (objectContent != null)
            {
                var type = objectContent.ObjectType;
                var value = objectContent.Value;
            }
            Logger.Info($"OnActionExecutedResponse {actionExecutedContext.Response.StatusCode.ToString()}");
        }
    }
}