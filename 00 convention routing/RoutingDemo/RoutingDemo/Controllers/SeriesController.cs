using RoutingDemo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RoutingDemo.Controllers
{
    public class SeriesController : ApiController
    {
        private static List<Serie> _series = new List<Serie>
            {
                new Serie
                {
                    Id = 1,
                    Title = "A",
                    Type = new Type { Id = 1, Description = "Drama" },
                },
                new Serie
                {
                    Id = 2,
                    Title = "B",
                    Type = new Type { Id = 2, Description = "Action" }
                },
            };

        // http://localhost:53135/api/series
        [ActionName("")]
        public IEnumerable<Serie> GetSeries()
        {
            return _series;
        }

        // http://localhost:53135/api/series/1
        [ActionName("")]
        public Serie GetSerieById(int id)
        {
            return _series.SingleOrDefault(s => s.Id == id);
        }

        // http://localhost:53135/api/series/2
        [ActionName("")]
        public HttpResponseMessage DeleteSerie(int id)
        {
            var serieToRemove = _series.SingleOrDefault(s => s.Id == id);

            if (serieToRemove == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            else
            {
                var index = _series.IndexOf(serieToRemove);
                _series.RemoveAt(index);
                return new HttpResponseMessage(HttpStatusCode.Accepted);
            }
        }

        // http://localhost:53135/api/series/findseriesbytype?typeId=1
        [HttpGet]
        public IEnumerable<Serie> FindSeriesByType(int typeId)
        {
            return _series
                .Where(s => s.Type.Id == typeId);
        }

        // http://localhost:53135/api/series/findbytitle?title=A
        [HttpGet]
        public Serie FindByTitle(string title)
        {
            return _series.SingleOrDefault(s => s.Title == title);
        }
    }
}
