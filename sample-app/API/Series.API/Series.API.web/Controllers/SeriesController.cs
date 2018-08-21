using Series.API.web.Filters;
using Series.Backend.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Series.API.web.Controllers
{
    public class SeriesController : ApiController
    {
        private IContainerRepositories _containerRepositories;

        public SeriesController(IContainerRepositories containerRepositories)
        {
            _containerRepositories = containerRepositories;
        }

        [Logger]
        // http://localhost:62608/api/series
        public IEnumerable<string> GetAllSeries()
        {
            var titles = _containerRepositories.SeriesRepository
                .GetSeries()
                .Select(s => s.Title)
                .ToList();

            return titles;
        }

        // http://localhost:62608/api/series?title=TitleA
        public IHttpActionResult GetSerie(string title)
        { 
            var found = _containerRepositories.SeriesRepository
                .GetSeries()
                .FirstOrDefault(s => s.Title == title);

            if (found == null)
            {
                return NotFound();
            }

            return Ok(found);
        }
    }
}
