using Serilog.Core;
using Serilog.Events;

namespace NetTemplate.Shared.Infrastructure.Logging.Filters
{
    public class FileLoggerFilter : ILogEventFilter
    {
        public bool IsEnabled(LogEvent logEvent)
        {
            var acceptedLogLevel = new[] { LogEventLevel.Error, LogEventLevel.Fatal };
            return acceptedLogLevel.Contains(logEvent.Level);
        }
    }
}
