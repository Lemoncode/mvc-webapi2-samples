using Series.Backend.Entities;
using Series.Backend.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace DatabaseInit
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new SeriesDbContextInitializer());

            using (var context = new SeriesDbContext())
            {
                context.Genres
                    .AddRange
                    (
                        new List<Genre>
                        {
                            new Genre
                            {
                                Id = 1,
                                Description = "Drama",
                            },
                            new Genre
                            {
                                Id = 2,
                                Description = "Comedy",
                            },
                            new Genre
                            {
                                Id = 3,
                                Description = "Fiction",
                            },
                            new Genre
                            {
                                Id = 4,
                                Description = "Action",
                            },
                        }
                    );

                context.TVSeries
                    .AddRange
                    (
                        new List<TVSerie>
                        {
                            new TVSerie
                            {
                                Complete = false,
                                Creator = "Foo bros",
                                OriginalRelease = new DateTime(2012, 2, 15),
                                GenreId = 3,
                                Title = "Game of bars"
                            },
                            new TVSerie
                            {
                                Complete = true,
                                Creator = "Doe Jane",
                                OriginalRelease = new DateTime(2002, 4, 20),
                                GenreId = 2,
                                Title = "Enemies"
                            },
                            new TVSerie
                            {
                                Complete = true,
                                Creator = "Joe Foo",
                                OriginalRelease = new DateTime(2000, 3, 16),
                                GenreId = 4,
                                Title = "The run away"
                            },
                            new TVSerie
                            {
                                Complete = false,
                                Creator = "Joe Foo",
                                OriginalRelease = new DateTime(2018, 7, 18),
                                GenreId = 4,
                                Title = "The run away, origins"
                            },
                            new TVSerie
                            {
                                Complete = false,
                                Creator = "James Foo",
                                OriginalRelease = new DateTime(2017, 4, 16),
                                GenreId = 1,
                                Title = "Desire"
                            },
                        }
                    );

                context.SaveChanges();
            }
        }
    }
}