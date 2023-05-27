using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using EnvironmentConstants = NetTemplate.Shared.WebApi.Common.Constants.Environment;

namespace NetTemplate.Shared.WebApi.Logging.Extensions
{
    public static class LoggingExtensions
    {
        public const string HostLevelLogFile = "logs/host/host.txt";
        public const string HostLevelLogTemplate = "[{UtcTimestamp} {Level:u3}] {Message:lj}{NewLine}{Exception}";

        public static LoggerConfiguration HostLevelLog(this LoggerSinkConfiguration writeTo)
        {
            var template = HostLevelLogTemplate;
            var env = Environment.GetEnvironmentVariable(EnvironmentConstants.VariableName);

            if (env != EnvironmentConstants.Production)
            {
                return writeTo.Console(restrictedToMinimumLevel: LogEventLevel.Information, outputTemplate: template);
            }

            return writeTo.File(HostLevelLogFile,
                rollingInterval: RollingInterval.Month,
                restrictedToMinimumLevel: LogEventLevel.Information, outputTemplate: template);
        }
    }
}
