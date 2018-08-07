using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
