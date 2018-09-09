using CustomFilter.Demo.Filters;
using System.Collections.Generic;
using System.Web.Http;

namespace CustomFilter.Demo.Controllers
{
    [Log]
    public class FooController : ApiController
    {
        // api/foo/
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
