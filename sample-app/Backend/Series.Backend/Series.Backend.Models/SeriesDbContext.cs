using Series.Backend.Entities;
using Series.Backend.Models.ModelConfigurations;
using System.Data.Entity;

namespace Series.Backend.Models
{
    public class SeriesDbContext : DbContext
    {
        public DbSet<Genre> Genres { get; set; }
        public DbSet<TVSerie> TVSeries { get; set; }

        public SeriesDbContext(): base("series"){}
        
        // TODO: Host project on GitHub
        // TODO: Create automated build with VSTS
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new GenreConfiguration());
            modelBuilder.Configurations.Add(new TVSerieConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
