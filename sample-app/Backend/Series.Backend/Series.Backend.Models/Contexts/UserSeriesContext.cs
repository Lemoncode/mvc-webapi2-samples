using Series.Backend.Entities;
using Series.Backend.Models.ModelConfigurations;
using System.Data.Entity;

namespace Series.Backend.Models.Contexts
{
    public class UserSeriesContext : DbContext
    {
        public DbSet<TVSerie> TVSeries { get; set; }
        public DbSet<TVSerieUser> TVSerieUser { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new TVSerieConfiguration());
            modelBuilder.Configurations.Add(new TVSerieUserConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());

            base.OnModelCreating(modelBuilder);
        }


        public UserSeriesContext() : base("series")
        {
            ContextInitialization();
        }

        public UserSeriesContext(string connString) : base(connString)
        {
            ContextInitialization();
        }

        private void ContextInitialization()
        {
            Configuration.LazyLoadingEnabled = false;
            Database.SetInitializer<UserSeriesContext>(null);
        }
    }
}
