using Serilog.Core;
using Serilog.Events;
using System.Diagnostics;

namespace NetTemplate.Shared.Infrastructure.Logging.Filters
{
    [DebuggerStepThrough]
    public class FileLoggerFilter : ILogEventFilter
    {
        public bool IsEnabled(LogEvent logEvent)
        {
            var acceptedLogLevel = new[] { LogEventLevel.Error, LogEventLevel.Fatal };
            return acceptedLogLevel.Contains(logEvent.Level);
        }
    }
}
