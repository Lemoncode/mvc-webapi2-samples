# In this demo we are going to create a custom filter for Web API

### 1. First we are going to create a new folder Filters, and place there the following code:

```C#
using System.Web.Http.Filters; // [1]

namespace CustomFilter.Demo.Filters
{
    public class Log : ActionFilterAttribute // [2]
    {

    }
}
```

1. Notice that we are not using the mvc namespace
2. We are extending from `ActionFilterAttribute`, this is interesting because expose to us methods that are hooks for before an after action gets executed.


### 2. We can implement this on async way

```C#
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
```

1. This allow us to have multiple instances of the same filter.
2. Because we have to return a Task, what we have done here is use `Task.Factory.StartNew` 

### 3. Now that we have this on place we can use it into our controllers.

* Create a new contoller, FooController

```C#
using CustomFilter.Demo.Filters;
using System.Collections.Generic;
using System.Web.Http;

namespace CustomFilter.Demo.Controllers
{
    [Log]
    public class FooController : ApiController
    {
        public IHttpActionResult GetFooThings()
        {
            var fooThings = new List<string>
            {
                "banana",
                "mouse",
                "people"
            };
            return Ok(fooThings);
        }
    }
}

```

* We can watch the results on the output debug window.