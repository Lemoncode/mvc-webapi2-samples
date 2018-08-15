using Series.Backend.Contracts;
using Series.Backend.Models;
using System;
using System.Linq;

namespace TestRunners.ContainerRepoistories
{
    class Program
    {
        static void Main(string[] args)
        {
            IContainerRepositoriesFactory containerRepositoriesFactory = new ContainerRepositoriesFactory();
            IContainerRepositories containerRepositories = new ContainerRepositories();
            ISeriesRepository seriesRepository = containerRepositories.SeriesRepository;
            seriesRepository.GetSeries()
                .ToList()
                .ForEach(s => { Console.WriteLine(s.Title); });
            Console.ReadKey();
        }
    }
}
