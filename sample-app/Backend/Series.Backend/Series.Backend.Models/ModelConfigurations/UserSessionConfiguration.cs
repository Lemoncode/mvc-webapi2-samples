using Series.Backend.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Series.Backend.Models.ModelConfigurations
{
    public class UserSessionConfiguration : EntityTypeConfiguration<UserSession>
    {
        public UserSessionConfiguration()
        {
            ToTable("UserSessions", "security");

            Property(u => u.Token)
                .HasMaxLength(88);
        }
    }
}
