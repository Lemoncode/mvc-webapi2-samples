using Series.Backend.Entities;
using Series.Backend.Models.ModelConfigurations;
using System.Data.Entity;

namespace Series.Backend.Models
{
    public class SeriesDbContext : DbContext
    {
        public DbSet<Genre> Genres { get; set; }
        public DbSet<TVSerie> TVSeries { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TVSerieUser> TVSerieUser { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }

        public SeriesDbContext(): base("series"){}
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new GenreConfiguration());
            modelBuilder.Configurations.Add(new TVSerieConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new TVSerieUserConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new UserProfileConfiguration());
            modelBuilder.Configurations.Add(new UserSessionConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
