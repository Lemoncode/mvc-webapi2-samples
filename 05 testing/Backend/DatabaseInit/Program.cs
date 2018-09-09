using Backend.Entities;
using Backend.Models;
using System;
using System.Collections.Generic;
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
                context.Series
                    .AddRange
                    (
                        new List<Serie>
                        {
                            new Serie
                            {
                                Id = 1,
                                Complete = false,
                                Creator = "Foo bros",
                                OriginalRelease = new DateTime(2012, 2, 15),

                                Title = "Game of bars"
                            },
                            new Serie
                            {
                                Id = 2,
                                Complete = true,
                                Creator = "Doe Jane",
                                OriginalRelease = new DateTime(2002, 4, 20),

                                Title = "Enemies"
                            },
                            new Serie
                            {
                                Id = 3,
                                Complete = true,
                                Creator = "Joe Foo",
                                OriginalRelease = new DateTime(2000, 3, 16),

                                Title = "The run away"
                            },
                            new Serie
                            {
                                Id = 4,
                                Complete = false,
                                Creator = "Joe Foo",
                                OriginalRelease = new DateTime(2018, 7, 18),

                                Title = "The run away, origins"
                            },
                            new Serie
                            {
                                Id = 5,
                                Complete = false,
                                Creator = "James Foo",
                                OriginalRelease = new DateTime(2017, 4, 16),

                                Title = "Desire"
                            },
                        }
                    );
                context.SaveChanges();
            }
        }
    }
}
