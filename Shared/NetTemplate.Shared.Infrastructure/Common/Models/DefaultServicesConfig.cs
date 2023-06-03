using NetTemplate.Redis.Models;
using NetTemplate.Shared.ClientSDK.Common.Models;
using NetTemplate.Shared.Infrastructure.Background.Models;
using NetTemplate.Shared.Infrastructure.Identity.Models;
using NetTemplate.Shared.Infrastructure.PubSub.Models;
using System.Reflection;

namespace NetTemplate.Shared.Infrastructure.Common.Models
{
    public class DefaultServicesConfig
    {
        // Common
        public Assembly[] ScanningAssemblies { get; set; }

        // DbContext
        public string DbContextConnectionString { get; set; }
        public bool DbContextDebugEnabled { get; set; }

        // Identity
        public IdentityConfig IdentityConfig { get; set; }

        // PubSubConfig
        public PubSubConfig PubSubConfig { get; set; }

        // Hangfire
        public string HangfireConnectionString { get; set; }
        public string HangfireMasterConnectionString { get; set; }
        public HangfireConfig HangfireConfig { get; set; }

        // Client SDK
        public ClientConfig ClientConfig { get; set; }

        // Redis
        public RedisConfig RedisConfig { get; set; }
    }
}
