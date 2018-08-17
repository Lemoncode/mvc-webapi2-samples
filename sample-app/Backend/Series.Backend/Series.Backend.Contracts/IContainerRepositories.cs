namespace Series.Backend.Contracts
{
    public interface IContainerRepositories
    {
        ISeriesRepository SeriesRepository { get; }
        IUserSeriesRepository UserSeriesRepository { get; }
    }
}
