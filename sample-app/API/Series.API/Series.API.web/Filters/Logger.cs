using System;
using System.Diagnostics;

namespace Series.API.web.Filters
{
    // TODO: Move to outer library
    public class Logger : ILogger
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