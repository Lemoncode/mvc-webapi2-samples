using Series.Backend.Contracts;
using Series.Backend.Entities;
using Series.Backend.Models.Contexts;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Series.Backend.Models.Repositories
{
    public class SeriesRepository : ISeriesRepository
    {
        private SeriesContext _context;

        public SeriesRepository()
        {
            _context = new SeriesContext();
        }

        public SeriesRepository(string connString)
        {
            _context = new SeriesContext(connString);
        }

        public TVSerie GetSerieById(int id)
        {
            return _context.TVSeries
                    .Where(t => t.Id == id)
                    .Select
                    (
                        s => new
                        {
                            s.Complete,
                            s.Creator,
                            s.GenreId,
                            s.Genre.Description,
                            s.Id,
                            s.OriginalRelease,
                            s.Title,
                        }
                    )
                    .AsNoTracking()
                    .ToList()
                    .Select
                    (
                        s => new TVSerie
                        {
                            Id = s.Id,
                            Complete = s.Complete,
                            Creator = s.Creator,
                            Genre = new Genre { Description = s.Description },
                            GenreId = s.GenreId,
                            OriginalRelease = s.OriginalRelease,
                            Title = s.Title,
                        }
                    )
                    .SingleOrDefault();
        }

        public IEnumerable<TVSerie> GetSeries()
        {
            return _context.TVSeries
                    .Select
                    (
                        s => new
                        {
                            s.Complete,
                            s.Creator,
                            s.GenreId,
                            s.Genre.Description,
                            s.Id,
                            s.OriginalRelease,
                            s.Title,
                        }
                    )
                    .AsNoTracking()
                    .ToList()
                    .Select
                    (
                        s => new TVSerie
                        {
                            Id = s.Id,
                            Complete = s.Complete,
                            Creator = s.Creator,
                            Genre = new Genre { Description = s.Description }  ,
                            GenreId = s.GenreId,
                            OriginalRelease = s.OriginalRelease,   
                            Title = s.Title,
                        }
                    );
        }
    }
}
