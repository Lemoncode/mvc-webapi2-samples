using System.Diagnostics;

namespace DIAdvancedScenarios.API.Filters
{
    public class LoggerService : ILogger
    {
        public void Info(string message)
        {
            Debug.WriteLine($"Information: {message}");
        }

        public void Warning(string message)
        {
            Debug.WriteLine($"Warning: {message}");
        }
    }
}