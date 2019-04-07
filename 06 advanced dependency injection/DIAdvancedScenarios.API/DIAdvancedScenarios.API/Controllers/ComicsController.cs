using DIAdvancedScenarios.API.Filters;
using System.Collections.Generic;
using System.Web.Http;

namespace DIAdvancedScenarios.API.Controllers
{
    public class ComicsController : ApiController
    {
        // api/comics
        [Logger]
        public IEnumerable<string> GetTitles()
        {
            return new string[] { "Batman", "Superman" };
        }
    }
}
