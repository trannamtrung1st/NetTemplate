using NetTemplate.Shared.Infrastructure.Logging.Filters;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace NetTemplate.Shared.Infrastructure.Logging.Extensions
{
    public static class LoggerConfigurationExtensions
    {
        public const string HostLevelLogFile = "logs/host/host.txt";
        public const string HostLevelLogTemplate = "[{UtcTimestamp} {Level:u3}] {Message:lj}{NewLine}{Exception}";

        public static LoggerConfiguration WithFileLoggerFilter(this LoggerFilterConfiguration filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            return filter.With<FileLoggerFilter>();
        }

        public static LoggerConfiguration HostLevelLog(this LoggerSinkConfiguration writeTo,
            bool isProduction)
        {
            var template = HostLevelLogTemplate;

            if (!isProduction)
            {
                return writeTo.Console(restrictedToMinimumLevel: LogEventLevel.Information, outputTemplate: template);
            }

            return writeTo.File(HostLevelLogFile,
                rollingInterval: RollingInterval.Month,
                restrictedToMinimumLevel: LogEventLevel.Information, outputTemplate: template);
        }
    }
}
