using Backend.Utils;
using Series.Backend.Contracts;
using Series.Backend.Models.Repositories;

namespace Series.Backend.Models
{
    public class ContainerRepositories : IContainerRepositories
    {
        private IRepositoryFactory<SeriesRepository> _seriesRepositoryFactory;

        public ContainerRepositories(string connectionString = null)
        {
            InitializeRepositoryFactories(connectionString);
        }

        private void InitializeRepositoryFactories(string connectionString = null)
        {
            _seriesRepositoryFactory = new RepositoryFactory<SeriesRepository>();

            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                _seriesRepositoryFactory.SetConnectionString(connectionString);
            }
        }

        public ISeriesRepository SeriesRepository => _seriesRepositoryFactory.CreateInstance();
    }
}
