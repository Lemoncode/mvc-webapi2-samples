using System.Data.Entity;

namespace Series.Backend.Models
{
    public class SeriesDbContextInitializer : DropCreateDatabaseAlways<SeriesDbContext>
    {
        protected override void Seed(SeriesDbContext context)
        {
            base.Seed(context);
        }
    }
}
