using Series.Backend.Contracts;

namespace Series.Backend.Models
{
    public class ContainerRepositoriesFactory : IContainerRepositoriesFactory
    {
        private string _connectionString;
        private bool _connectionStringSet = false;

        public IContainerRepositories CreateInstance()
        {
            return _connectionStringSet ?  new ContainerRepositories(_connectionString) : new ContainerRepositories();
        }

        public void SetConnectionString(string connectionString)
        {
            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                _connectionString = connectionString;
                _connectionStringSet = true;
            }
        }
    }
}
