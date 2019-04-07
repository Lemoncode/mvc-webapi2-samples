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