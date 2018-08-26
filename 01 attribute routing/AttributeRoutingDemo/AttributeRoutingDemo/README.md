# In this demo we are going to use attribute routing

>NOTE: Start a new empty web project and select Web API option for scaffolding.

TODO: Create slides about REST

> Reference: https://www.codecademy.com/articles/what-is-rest
> Reference: https://static.googleusercontent.com/media/research.google.com/es//pubs/archive/46310.pdf

## Intro

* Attribute routing uses attributes to define routes. 
* Attribute routing gives you more control over the URIs in your web API. For example, you can easily create URIs that describe hierarchies of resources.
* The earlier style of routing, called convention-based routing, is still fully supported. In fact, you can combine both techniques in the same project.

* One advantage of convention-based routing is that templates are defined in a single place, and the routing rules are applied consistently across all controllers. 
* Unfortunately, convention-based routing makes it hard to support certain URI patterns that are common in RESTful APIs. 
* For example, resources often contain child resources: Customers have orders, movies have actors, books have authors, and so forth. It's natural to create URIs that reflect these relations:
	- `users/1/series`

* Create this with attribute routing it's trivial

```C#
[Route("users/{userId}/series")]
public IEnumerable<Serie> GetUserSeries(int userId) {...}
```
* We can make easy versioning: 
	- `api/v1/products`
	- `api/v2/products`
* Overload URI segments: 
	- `/users/1`
	- `/users/active`
* Multiple parameter types:
	- `/users/1`
	- `/users/2010/06/16`

* To eneble the attribute routing, on our WebApiConfig.cs

```C#
public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes(); // We have to pass this.

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
```

## 1. Let's show an example.
* Create models:

```C# Serie.cs
namespace AttributeRoutingDemo.Models
{
    public class Serie
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
```

```C# User.cs
using System.Collections.Generic;

namespace AttributeRoutingDemo.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Serie> UserSeries { get; set; }
    }
}
```

```C# UserController.cs
using AttributeRoutingDemo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace AttributeRoutingDemo.Controllers
{
    public class UserController : ApiController
    {
        private static List<User> _users = new List<User>
        {
            new User
            {
                Id = 1,
                Name = "Jane",
                UserSeries = new List<Serie>
                {
                    new Serie
                    {
                        Id = 1,
                        Title = "A",
                    },
                    new Serie
                    {
                        Id = 2,
                        Title = "B",
                    },
                }
            },
            new User
            {
                Id = 2,
                Name = "Joe",
                UserSeries = new List<Serie>
                {
                    new Serie
                    {
                        Id = 1,
                        Title = "A",
                    }
                }
            },
        };

        [Route("api/users/{userId}/series")]
        [HttpGet]
        public IEnumerable<Serie> GetUserSeriesByUserId(int userId)
        {
            return _users.SingleOrDefault(u => u.Id == userId)
                .UserSeries;
        }
    }
}

```

* What will happen if we remove the `[HttpGet]` attribute?

```diff
  [Route("api/users/{userId}/series")]
- [HttpGet]
  public IEnumerable<Serie> GetUserSeriesByUserId(int userId)
  {
      return _users.SingleOrDefault(u => u.Id == userId)
          .UserSeries;
  }
```

* It still working because, by default, Web API looks for a case-insensitive match with the start of the controller method name. 
* For example, a controller method named PutCustomers matches an HTTP PUT request.
* You can override this convention by decorating the method with any the following attributes:
	- [HttpDelete]
	- [HttpGet]
	- [HttpHead]
	- [HttpOptions]
	- [HttpPatch]
	- [HttpPost]
	- [HttpPut]

## 2. Setting route prefixes

* First start  by creating a new action:

```diff
using AttributeRoutingDemo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace AttributeRoutingDemo.Controllers
{
    public class UserController : ApiController
    {
        private static List<User> _users = new List<User>
        {
            new User
            {
                Id = 1,
                Name = "Jane",
                UserSeries = new List<Serie>
                {
                    new Serie
                    {
                        Id = 1,
                        Title = "A",
                    },
                    new Serie
                    {
                        Id = 2,
                        Title = "B",
                    },
                }
            },
            new User
            {
                Id = 2,
                Name = "Joe",
                UserSeries = new List<Serie>
                {
                    new Serie
                    {
                        Id = 1,
                        Title = "A",
                    }
                }
            },
        };

        [Route("api/users/{userId}/series")]
        // [HttpGet]
        public IEnumerable<Serie> GetUserSeriesByUserId(int userId)
        {
            return _users.SingleOrDefault(u => u.Id == userId)
                .UserSeries;
        }

+        [HttpPost]
+        [Route("api/users/signin")]
+        public IHttpActionResult SignIn(object userCredentials)
+        {
+            if (userCredentials != null)
+            {
+                return Ok("Grant access");
+            }
+
+            return Unauthorized();
+        } 
    }
}

```

* We can notice that all our actions in this controller are using the api prefix.
* We can set a common prefix for an entire controller by using the [RoutePrefix] attribute:

```diff
using AttributeRoutingDemo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace AttributeRoutingDemo.Controllers
{
+   [RoutePrefix("api/users")]
    public class UserController : ApiController
    {
        private static List<User> _users = new List<User>
        {
            new User
            {
                Id = 1,
                Name = "Jane",
                UserSeries = new List<Serie>
                {
                    new Serie
                    {
                        Id = 1,
                        Title = "A",
                    },
                    new Serie
                    {
                        Id = 2,
                        Title = "B",
                    },
                }
            },
            new User
            {
                Id = 2,
                Name = "Joe",
                UserSeries = new List<Serie>
                {
                    new Serie
                    {
                        Id = 1,
                        Title = "A",
                    }
                }
            },
        };

-       [Route("api/users/{userId}/series")]
+       [Route("{userId}/series")]
        public IEnumerable<Serie> GetUserSeriesByUserId(int userId)
        {
            return _users.SingleOrDefault(u => u.Id == userId)
                .UserSeries;
        }

        [HttpPost]
-       [Route("api/users/signin")]
+       [Route("signin")]
        public IHttpActionResult SignIn(object userCredentials)
        {
            if (userCredentials != null)
            {
                return Ok("Grant access");
            }

            return Unauthorized();
        } 
    }
}

```
* We can use ~ to override.
* The route prefix can include parameters.

> NOTE: We can apply constraints to our routes.

## 3. We can make parameters optional

```C#
[HttpGet]
[Route("{userId}/series/favorite/{serieId:int?}")]
public Serie FavaoriteUserSerie(int userId, int serieId = 1)
{
    return _users.SingleOrDefault(u => u.Id == userId)
            .UserSeries
            .Where(us => us.Id == serieId)
            .FirstOrDefault();
}
```

* Be aware that optional parameters have to come at last, otherwise the route will not be build as Web Api expecs.

