using Series.Backend.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Series.Backend.Models.ModelConfigurations
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            ToTable("Users");

            Property(u => u.Name)
                .HasMaxLength(80);

            Property(u => u.Email)
                .HasMaxLength(160);

            HasMany(u => u.TVSerieUserCollection)
                .WithRequired(tvs => tvs.User)
                .HasForeignKey(tvs => tvs.UserId)
                .WillCascadeOnDelete(false);
        }
    }
}
