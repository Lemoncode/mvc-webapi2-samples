namespace Series.Backend.Contracts
{
    public interface IContainerRepositoriesFactory
    {
        void SetConnectionString(string connectionString);
        IContainerRepositories CreateInstance();
    }
}
