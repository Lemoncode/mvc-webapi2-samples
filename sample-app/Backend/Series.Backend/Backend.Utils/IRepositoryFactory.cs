namespace Backend.Utils
{
    public interface IRepositoryFactory<T>
    {
        void SetConnectionString(string connectionString);
        T CreateInstance();
    }
}
