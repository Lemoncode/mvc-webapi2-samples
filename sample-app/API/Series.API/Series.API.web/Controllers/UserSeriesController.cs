using Series.Backend.Contracts;
using Series.Backend.Entities;
using Series.RestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Series.API.web.Controllers
{
    public class UserSeriesController : ApiController
    {
        private IContainerRepositories _containerRepositories;

        public UserSeriesController(IContainerRepositories containerRepositories)
        {
            _containerRepositories = containerRepositories;
        }

        // http://localhost:62608/api/userseries?userId=1
        public IEnumerable<UserSerie> GetUserSeries(int userId)
        {
            var series = _containerRepositories.SeriesRepository.GetSeries();
            var userSeries = _containerRepositories.UserSeriesRepository.GetUserSeries(userId);
            IEnumerable<int> notFollowingByUserSeriesIds = SeriesIdsNotFollowByUser(series, userSeries);

            var userSeriesNotFollowing = MapToUserSerieCollectionNotFollowing(series, notFollowingByUserSeriesIds);
            var userSeriesFollowing = MapToUserSerieCollectionFollowing(userSeries);

            return userSeriesNotFollowing
                    .Concat(userSeriesFollowing)
                    .OrderBy(s => s.Id);
        }

        // http://localhost:62608/api/user/1/series/2
        [Route("api/user/{userId}/series/{serieId}")]
        [HttpPost]
        public IHttpActionResult UserFollowSerie(int userId, int serieId)
        {
            try
            {
                _containerRepositories.UserSeriesRepository.AddUserSerie(userId, serieId);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        private IEnumerable<UserSerie> MapToUserSerieCollectionFollowing(IEnumerable<TVSerie> userSeries)
        {
            return userSeries.Select
                                (
                                    s => new UserSerie
                                    {
                                        Id = s.Id,
                                        Complete = s.Complete,
                                        Following = false,
                                        Genre = s.Genre.Description,
                                        Title = s.Title,
                                    }
                                );
        }

        private IEnumerable<UserSerie> MapToUserSerieCollectionNotFollowing(IEnumerable<TVSerie> series, IEnumerable<int> notFollowingByUserSeriesIds)
        {
            return series
                    .Where
                    (
                        s => notFollowingByUserSeriesIds.Any(n => n == s.Id)
                    )
                    .Select
                    (
                        s => new UserSerie
                        {
                            Id = s.Id,
                            Complete = s.Complete,
                            Following = false,
                            Genre = s.Genre.Description,
                            Title = s.Title,
                        }
                    );
        }

        private static IEnumerable<int> SeriesIdsNotFollowByUser(IEnumerable<TVSerie> series, IEnumerable<TVSerie> userSeries)
        {
            var seriesIds = series.Select(s => s.Id);
            var userSeriesIds = userSeries.Select(u => u.Id);
            var notFollowingByUserSeriesIds = seriesIds.Except(userSeriesIds);
            return notFollowingByUserSeriesIds;
        }
    }
}
