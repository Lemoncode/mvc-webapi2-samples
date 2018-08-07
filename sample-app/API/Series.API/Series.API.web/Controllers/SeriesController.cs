using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Series.API.web.Controllers
{
    public class SeriesController : ApiController
    {
        List<string> titles = new List<string>
        {
            "TitleA",
            "TitleB",
            "TitleC",
            "TitleD",
            "TitleE",
            "TitleF",
        };

        // http://localhost:62608/api/series
        public IEnumerable<string> GetAllSeries()
        {
            return titles;
        }

        // http://localhost:62608/api/series?title=TitleA
        public IHttpActionResult GetSerie(string title)
        {
            var found = titles.FirstOrDefault(t => t == title);
            if (found == null)
            {
                return NotFound();
            }
            return Ok(found); 
        }
    }
}
