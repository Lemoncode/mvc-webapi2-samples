using Series.Backend.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Series.Backend.Models.ModelConfigurations
{
    public class TVSerieUserConfiguration : EntityTypeConfiguration<TVSerieUser>
    {
        public TVSerieUserConfiguration()
        {
            ToTable("TVSerieUsers");

            HasRequired(su => su.TVSerie)
                .WithMany(tvs => tvs.TVSerieUserCollection)
                .HasForeignKey(su => su.TVSerieId)
                .WillCascadeOnDelete(false);
        }
    }
}
