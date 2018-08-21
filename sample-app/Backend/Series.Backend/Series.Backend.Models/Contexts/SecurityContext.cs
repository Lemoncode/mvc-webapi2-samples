using Series.Backend.Entities;
using Series.Backend.Models.ModelConfigurations;
using System.Data.Entity;

namespace Series.Backend.Models.Contexts
{
    public class SecurityContext : DbContext
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new UserProfileConfiguration());
            modelBuilder.Configurations.Add(new UserSessionConfiguration());
        }

        public SecurityContext() : base("series")
        {
            ContextInitialization();
        }

        public SecurityContext(string connString) : base(connString)
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
