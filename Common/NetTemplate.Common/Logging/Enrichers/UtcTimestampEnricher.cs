using Serilog.Core;
using Serilog.Events;

namespace Serilog
{
    public class UtcTimestampEnricher : ILogEventEnricher
    {
        public const string UtcTimestampProperty = "UtcTimestamp";

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory pf)
        {
            logEvent.AddPropertyIfAbsent(pf.CreateProperty(UtcTimestampProperty, logEvent.Timestamp.UtcDateTime));
        }
    }
}
