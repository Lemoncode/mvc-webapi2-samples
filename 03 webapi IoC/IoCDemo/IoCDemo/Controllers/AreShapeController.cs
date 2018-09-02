using Shapes.Contracts;
using System.Web.Http;

namespace IoCDemo.Controllers
{
    public class AreShapeController : ApiController
    {
        private IAreaResolver _areaResolverService;

        public AreShapeController(IAreaResolver areaResolverService)
        {
            _areaResolverService = areaResolverService;
        }

        [Route("shapes/circle/{radius}/area")]
        [HttpGet]
        public IHttpActionResult CircleArea(string radius)
        {
            double _radius;
            if (double.TryParse(radius, out _radius))
            {
                return Ok(_areaResolverService.CircleArea(_radius));
            }

            return BadRequest($"Not valid radius: {radius}");
        }
    }
}
