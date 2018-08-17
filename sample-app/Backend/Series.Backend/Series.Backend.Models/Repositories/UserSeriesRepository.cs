using Series.Backend.Contracts;
using Series.Backend.Entities;
using Series.Backend.Models.Contexts;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Series.Backend.Models.Repositories
{
    public class UserSeriesRepository : IUserSeriesRepository
    {
        private UserSeriesContext _context;

        public UserSeriesRepository()
        {
            _context = new UserSeriesContext();
        }

        public UserSeriesRepository(string connString)
        {
            _context = new UserSeriesContext(connString);
        }

        public IEnumerable<TVSerie> GetUserSeries(int userId)
        {
            List<TVSerie> userSeries = null;
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            if (user != null)
            {
                var result = _context.TVSerieUser
                    .Where(tvs => tvs.UserId == user.Id)
                    .Select
                    (
                        tvs => new
                        {
                            tvs.TVSerie.Id,
                            tvs.TVSerie.Title,
                            tvs.TVSerie.Complete,
                            GenreDescription = tvs.TVSerie.Genre.Description,
                        }
                    )
                    .AsNoTracking()
                    .ToList()
                    .Select
                    (
                        tvs => new TVSerie
                        {
                            Id = tvs.Id,
                            Title = tvs.Title,
                            Complete = tvs.Complete,
                            Genre = new Genre { Description = tvs.GenreDescription }
                        }
                    );

                userSeries = new List<TVSerie>(result);
            }

            return userSeries;
        }

        public void AddUserSerie(int userId, int serieId)
        {
            var TVSerieUser = _context.TVSerieUser.SingleOrDefault(tvs => tvs.UserId == userId && tvs.TVSerieId == serieId);
            if (TVSerieUser == null)
            {
                var newSerieUser = new TVSerieUser { UserId = userId, TVSerieId = serieId };
                _context.TVSerieUser.Add(newSerieUser);
                _context.SaveChanges();
            }
            else
            {
                // TODO: Throw custom exception
            }
        }
    }
}
