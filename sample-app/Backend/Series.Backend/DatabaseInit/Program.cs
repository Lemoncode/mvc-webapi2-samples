using Series.Backend.Models;
using System.Data.Entity;
using System.Linq;

namespace DatabaseInit
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new SeriesDbContextInitializer());

            using (var context = new SeriesDbContext())
            {
                context.TVSeries.ToList();
            }
        }
    }
}
