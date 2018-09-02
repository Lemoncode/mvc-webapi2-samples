using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApiSetupDemo.Models;

namespace WebApiSetupDemo.Controllers
{
    // [EnableCors(origins:"*", headers:"*", methods:"*")]
    public class SeriesController : ApiController
    {
        static List<string> _titles = new List<string> { "A", "B" };

        // http://localhost:65423/api/series
        public IEnumerable<string> GetSeriesTitles()
        {
            return _titles;
        }

        // 
        [Route("api/series/complete")]
        public IEnumerable<Serie> GetSeries()
        {
            return new List<Serie> { new Serie { Id = 1, Title = "A" } };
        } 
    }
}
