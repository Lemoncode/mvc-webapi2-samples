using Backend.Entities;
using System.Data.Entity;

namespace Backend.Models
{
    public class SeriesDbContext : DbContext
    {
        public DbSet<Serie> Series { get; set; }

        public SeriesDbContext(): base("simpleseries")
        {
        }
    }
}
