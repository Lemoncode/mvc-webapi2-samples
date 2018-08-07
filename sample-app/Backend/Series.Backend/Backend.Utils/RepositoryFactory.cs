using System;

namespace Backend.Utils
{
    public class RepositoryFactory<T> : IRepositoryFactory<T> where T : new()
    {
        private string _connectionString;

        public T CreateInstance()
        {
            return string.IsNullOrWhiteSpace(_connectionString) ?
                new T() :
                (T)Activator.CreateInstance(typeof(T), _connectionString);
        }

        public void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }
    }
}
