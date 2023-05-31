using NetTemplate.Shared.Infrastructure.Logging.Extensions;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace NetTemplate.Shared.Infrastructure.Common.Utils
{
    public static class InfrastructureHelper
    {
        public static Logger CreateHostLogger(bool isProduction) => new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.WithUtcTimestamp()
                .WriteTo.HostLevelLog(isProduction)
                .CreateLogger();

        public static void CleanResources(IEnumerable<IDisposable> resources)
        {
            foreach (var resource in resources)
                resource.Dispose();
        }
    }
}
