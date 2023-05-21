using NetTemplate.Shared.Infrastructure.Logging.Filters;
using Serilog;
using Serilog.Configuration;

namespace NetTemplate.Shared.Infrastructure.Logging.Extensions
{
    public static class LoggerConfigurationExtensions
    {
        public static LoggerConfiguration WithFileLoggerFilter(this LoggerFilterConfiguration filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            return filter.With<FileLoggerFilter>();
        }
    }
}
