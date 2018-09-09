using Backend.Contracts;
using Backend.Entities;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace TestingDemo.Controllers
{
    public class SeriesController : ApiController
    {
        private ISeriesRepository _repository;

        public SeriesController(ISeriesRepository repository)
        {
            _repository = repository;
        }

        // TODO: Add unit test
        [HttpGet]
        [Route("series/complete")]
        public IHttpActionResult RetriveCompleteSeries()
        {
            var result = _repository.GetSeries()
                .AsQueryable()
                .Where(s => s.Complete);
            
            return Ok(result);
        }

        // TODO: Add unit test
        [HttpGet]
        [Route("series/complete/titles")]
        public IHttpActionResult RetriveCompleteSeriesTitles()
        {
            // TODO: Avoid read all Serie fields.
            // Version 1 Read all Serie table fields and we only want a subset
            //var result = _repository.GetSeries()
            //    .AsQueryable()
            //    .Where(s => s.Complete)
            //    .Select(s => s.Title);

            // Version 2 Read all Serie table fields and we only want a subset
            var result = _repository.GetSeries()
                .AsQueryable()
                .Where(s => s.Complete)
                .Select(s => new { s.Title })
                .ToList()
                .Select(s => new Serie { Title = s.Title });

            return Ok(result);
        }

        [HttpGet]
        [Route("series")]
        public IHttpActionResult RetrieveAllSeries()
        {
            var result = _repository.GetSeries();
            if (result == null)
            {
                return Ok("no series to display"); // Refactor this test.
            }
            return Ok(result);
        }
        
        [HttpGet]
        [Route("series/{id}")]
        public async Task<IHttpActionResult> RetrieveSerieById(int id)
        {
            var serie = await _repository.GetSerieByIdAsync(id);
            if (serie == null)
            {
                return NotFound();
            }

            return Ok(serie);
        }

        protected override void Dispose(bool disposing)
        {
            _repository.Dispose();
        }
    }
}
