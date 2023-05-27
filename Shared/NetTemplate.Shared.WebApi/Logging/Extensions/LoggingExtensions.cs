using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace NetTemplate.Shared.WebApi.Logging.Extensions
{
    public static class LoggingExtensions
    {
        public const string HostLevelLogFile = "logs/host/host.txt";
        public const string HostLevelLogTemplate = "[{UtcTimestamp} {Level:u3}] {Message:lj}{NewLine}{Exception}";

        public static LoggerConfiguration HostLevelLog(this LoggerSinkConfiguration writeTo)
        {
            var template = HostLevelLogTemplate;
            var env = Environment.GetEnvironmentVariable(SharedApiConstants.Environment.VariableName);

            if (env != SharedApiConstants.Environment.Production)
            {
                return writeTo.Console(restrictedToMinimumLevel: LogEventLevel.Information, outputTemplate: template);
            }

            return writeTo.File(HostLevelLogFile,
                rollingInterval: RollingInterval.Month,
                restrictedToMinimumLevel: LogEventLevel.Information, outputTemplate: template);
        }
    }
}
