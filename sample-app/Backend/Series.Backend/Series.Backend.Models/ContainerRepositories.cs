using Backend.Utils;
using Series.Backend.Contracts;
using Series.Backend.Models.Repositories;

namespace Series.Backend.Models
{
    public class ContainerRepositories : IContainerRepositories
    {
        private IRepositoryFactory<SeriesRepository> _seriesRepositoryFactory;
        private IRepositoryFactory<UserSeriesRepository> _userSeriesRepositoryFactory;
        private IRepositoryFactory<SecurityRepository> _securityRepositoryFactory;

        public ContainerRepositories(string connectionString = null)
        {
            InitializeRepositoryFactories(connectionString);
        }

        private void InitializeRepositoryFactories(string connectionString = null)
        {
            _seriesRepositoryFactory = new RepositoryFactory<SeriesRepository>();
            _userSeriesRepositoryFactory = new RepositoryFactory<UserSeriesRepository>();
            _securityRepositoryFactory = new RepositoryFactory<SecurityRepository>();

            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                _seriesRepositoryFactory.SetConnectionString(connectionString);
                _userSeriesRepositoryFactory.SetConnectionString(connectionString);
                _securityRepositoryFactory.SetConnectionString(connectionString);
            }
        }

        public ISeriesRepository SeriesRepository => _seriesRepositoryFactory.CreateInstance();
        public IUserSeriesRepository UserSeriesRepository => _userSeriesRepositoryFactory.CreateInstance();
        public ISecurityRepository SecurityRepository => _securityRepositoryFactory.CreateInstance();
    }
}
