using Series.Backend.Entities;
using Series.Backend.Models.ModelConfigurations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Series.Backend.Models.Contexts
{
    public class SeriesContext : DbContext
    {
        public DbSet<Genre> Genres { get; set; }
        public DbSet<TVSerie> TVSeries { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new GenreConfiguration());
            modelBuilder.Configurations.Add(new TVSerieConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public SeriesContext() : base("series")
        {
            ContextInitialization();
        }

        public SeriesContext(string connString) : base(connString)
        {
            ContextInitialization();
        }

        private void ContextInitialization()
        {
            Configuration.LazyLoadingEnabled = false;
            Database.SetInitializer<SeriesContext>(null);
        }
    }
}
