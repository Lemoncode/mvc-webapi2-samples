using Series.Backend.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Series.Backend.Models.ModelConfigurations
{
    public class UserProfileConfiguration : EntityTypeConfiguration<UserProfile>
    {
        public UserProfileConfiguration()
        {
            ToTable("UserProfiles", "security");

            Property(p => p.PasswordHash)
                .HasMaxLength(88);

            Property(p => p.Salt)
                .HasMaxLength(32);

            Property(u => u.Email)
                .HasMaxLength(80);

            Property(u => u.FirstName)
                .HasMaxLength(80);

            Property(u => u.LastName)
                .HasMaxLength(80);

            HasRequired(u => u.Role)
                .WithMany(p => p.UserProfiles)
                .HasForeignKey(c => c.RoleId)
                .WillCascadeOnDelete(false);

            HasMany(e => e.UserSessions)
                .WithRequired(u => u.UserProfile)
                .HasForeignKey(u => u.UserProfileId)
                .WillCascadeOnDelete(false);
        }
    }
}
