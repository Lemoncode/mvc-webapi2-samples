using System;
using System.Data.Entity;
using System.Linq;
using backendModels = Series.Backend.Models;

namespace TestRunners.SeriesDbContext
{
    class Program
    {
        static void Main(string[] args)
        {
            // NestedLINQAntiPattern();
            //UseInclude();
            UseSelectMany();
            Console.ReadKey();
        }

        private static void UseSelectMany()
        {
            using (var ctx = new backendModels.SeriesDbContext())
            {
                var t = ctx.Users.Where(u => u.LastName == "Doe")
                    .SelectMany(d => d.TVSerieUserCollection)
                    .Select(s => s.TVSerie)
                    .ToList();
            }
        }

        private static void UseInclude()
        {
            using (var ctx = new backendModels.SeriesDbContext())
            {
                var user = ctx.Users.Where(u => u.LastName == "Doe")
                    .Include("TVSerieUserCollection")
                    .ToList();
            }
        }

        private static void NestedLINQAntiPattern()
        {
            using (var ctx = new backendModels.SeriesDbContext())
            {
                var users = ctx.Users.Where(u => u.LastName == "Doe");
                foreach (var user in users)
                {
                    var tvSerieCol = ctx.TVSerieUser.Where(s => s.UserId == user.Id);
                    foreach (var tvSerieUser in tvSerieCol)
                    {
                        var serie = ctx.TVSeries.SingleOrDefault(s => s.Id == tvSerieUser.TVSerieId);
                        Console.WriteLine(serie.Title);
                    }
                }
            }
        }
    }
}
