using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;

namespace NetTemplate.Common.Logging.Extensions
{
    public static class ConfigurationExtensions
    {
        public static Logger ParseLogger(this IConfiguration configuration,
            string sectionName, IServiceProvider provider = null)
        {
            var loggerConfig = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration, sectionName);

            if (provider != null)
                loggerConfig = loggerConfig.ReadFrom.Services(provider);

            return loggerConfig.CreateLogger();
        }
    }
}
