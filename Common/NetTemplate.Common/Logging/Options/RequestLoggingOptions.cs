using Serilog.Events;

namespace NetTemplate.Common.Logging.Options
{
    public class RequestLoggingOptions
    {
        public string MessageTemplate { get; set; } =
            "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
        public LogEventLevel? StaticGetLevel { get; set; }
        public IDictionary<string, string> EnrichHeaders { get; set; } = new Dictionary<string, string>();
        public bool IncludeHost { get; set; } = false;
        public bool UseDefaultLogger { get; set; } = true;
    }
}
