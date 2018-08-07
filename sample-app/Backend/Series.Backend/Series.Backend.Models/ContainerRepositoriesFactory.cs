using Series.Backend.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Series.Backend.Models
{
    public class ContainerRepositoriesFactory : IContainerRepositoriesFactory
    {
        private string _connectionString;
        private bool _connectionStringSet = false;

        public IContainerRepositories CreateInstance()
        {
            return null;
        }

        public void SetConnectionString(string connectionString)
        {
            throw new NotImplementedException();
        }
    }
}
