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
                PopulateGenres(context);
                PopulateTVSeries(context);
                PopulateUsers(context);
                PopulateTVSerieUsers(context);

                context.SaveChanges();
            }
        }

        private static void PopulateTVSerieUsers(SeriesDbContext context)
        {
            context.TVSerieUser
                                .AddRange
                                (
                                    new List<TVSerieUser>
                                    {
                            new TVSerieUser
                            {
                                TVSerieId = 1,
                                UserId = 1,
                            },
                            new TVSerieUser
                            {
                                TVSerieId = 3,
                                UserId = 1,
                            },
                            new TVSerieUser
                            {
                                TVSerieId = 2,
                                UserId = 2,
                            },
                                    }
                                );
        }

        private static void PopulateUsers(SeriesDbContext context)
        {
            context.Users
                            .AddRange
                            (
                                new List<User>
                                {
                        new User
                        {
                            Id = 1,
                            Name = "Jane Doe",
                            Email = "jane.doe@foo.com",
                        },
                        new User
                        {
                            Id = 2,
                            Name = "Joe Doe",
                            Email = "joe.doe@foo.com",
                        },
                                }
                            );
        }

        private static void PopulateTVSeries(SeriesDbContext context)
        {
            context.TVSeries
                                .AddRange
                                (
                                    new List<TVSerie>
                                    {
                            new TVSerie
                            {
                                Id = 1,
                                Complete = false,
                                Creator = "Foo bros",
                                OriginalRelease = new DateTime(2012, 2, 15),
                                GenreId = 3,
                                Title = "Game of bars"
                            },
                            new TVSerie
                            {
                                Id = 2,
                                Complete = true,
                                Creator = "Doe Jane",
                                OriginalRelease = new DateTime(2002, 4, 20),
                                GenreId = 2,
                                Title = "Enemies"
                            },
                            new TVSerie
                            {
                                Id = 3,
                                Complete = true,
                                Creator = "Joe Foo",
                                OriginalRelease = new DateTime(2000, 3, 16),
                                GenreId = 4,
                                Title = "The run away"
                            },
                            new TVSerie
                            {
                                Id = 4,
                                Complete = false,
                                Creator = "Joe Foo",
                                OriginalRelease = new DateTime(2018, 7, 18),
                                GenreId = 4,
                                Title = "The run away, origins"
                            },
                            new TVSerie
                            {
                                Id = 5,
                                Complete = false,
                                Creator = "James Foo",
                                OriginalRelease = new DateTime(2017, 4, 16),
                                GenreId = 1,
                                Title = "Desire"
                            },
                                    }
                                );
        }

        private static void PopulateGenres(SeriesDbContext context)
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
        }
    }
}