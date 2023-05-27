using Microsoft.Extensions.Configuration;
using NetTemplate.Shared.Infrastructure.Identity.Models;

namespace NetTemplate.Shared.Infrastructure.Identity.Extensions
{
    public static class ConfigurationExtensions
    {
        // [TODO]
        public static IdentityConfig GetIdentityConfigDefaults(this IConfiguration configuration)
            => new IdentityConfig();
    }
}
