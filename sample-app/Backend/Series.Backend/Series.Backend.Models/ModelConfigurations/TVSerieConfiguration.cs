using Series.Backend.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Series.Backend.Models.ModelConfigurations
{
    public class TVSerieConfiguration : EntityTypeConfiguration<TVSerie>
    {
        public TVSerieConfiguration()
        {
            ToTable("TVseries");

            Property(s => s.Creator)
                .HasMaxLength(80);

            Property(s => s.Title)
                .HasMaxLength(160);

            HasRequired(s => s.Genre)
                .WithMany(g => g.TVSerieCollection)
                .HasForeignKey(s => s.GenreId)
                .WillCascadeOnDelete(false);
        }
    }
}
