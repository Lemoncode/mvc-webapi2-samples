using AttributeRoutingDemo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace AttributeRoutingDemo.Controllers
{
    [RoutePrefix("api/users")]
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
        
        [Route("{userId}/series")]
        public IEnumerable<Serie> GetUserSeriesByUserId(int userId)
        {
            return _users.SingleOrDefault(u => u.Id == userId)
                .UserSeries;
        }

        [HttpPost]
        [Route("signin")]
        public IHttpActionResult SignIn(object userCredentials)
        {
            if (userCredentials != null)
            {
                return Ok("Grant access");
            }

            return Unauthorized();
        }

        [HttpGet]
        [Route("{userId}/series/favorite/{serieId:int?}")]
        public Serie FavaoriteUserSerie(int userId, int serieId = 1)
        {
            return _users.SingleOrDefault(u => u.Id == userId)
                    .UserSeries
                    .Where(us => us.Id == serieId)
                    .FirstOrDefault();
        }
    }
}
