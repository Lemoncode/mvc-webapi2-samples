namespace Series.API.web.Filters
{
    // TODO: Move to outer library
    public interface ILogger
    {
        void Info(string message);
        void Warning(string message);
    }
}
