using Microsoft.Extensions.Hosting;
using NetTemplate.Shared.Infrastructure.Common.Extensions;

namespace NetTemplate.Shared.Infrastructure.Common.Utils
{
    public static class HostHelper
    {
        public static IHostBuilder CreateDefaultBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder => builder.InsertSharedJson())
                .UseDefault();
        }
    }
}
