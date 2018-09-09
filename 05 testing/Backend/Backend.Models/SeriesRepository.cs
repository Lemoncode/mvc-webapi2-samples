using Backend.Contracts;
using Backend.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class SeriesRepository : ISeriesRepository
    {
        private SeriesDbContext _context;

        public SeriesRepository()
        {
            _context = new SeriesDbContext();
        }
        
        public IEnumerable<Serie> GetSeries()
        {
            return _context.Series.ToList();
        }

        public Task<Serie> GetSerieByIdAsync(int id)
        {
            return _context.Series.FindAsync(id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
