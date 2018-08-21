using Series.Backend.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Series.Backend.Models.ModelConfigurations
{
    public class RoleConfiguration : EntityTypeConfiguration<Role>
    {
        public RoleConfiguration()
        {
            ToTable("Roles", "security");

            Property(r => r.Name)
                .HasMaxLength(80);
        }
    }
}
