using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetTemplate.Blog.ApplicationCore.Common.Extensions;
using NetTemplate.Blog.Infrastructure.Persistence;
using NetTemplate.Redis.Extensions;
using NetTemplate.Shared.Infrastructure.Common.Extensions;
using NetTemplate.Shared.Infrastructure.Common.Models;
using NetTemplate.Shared.Infrastructure.MemoryStore.Extensions;

namespace NetTemplate.Blog.Infrastructure.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
            DefaultServicesConfig config, IConfiguration configuration, bool isProduction)
        {
            return services.AddInfrastructureDefaultServices<MainDbContext>(config, isProduction)
                .ConfigureViewConfigs(configuration)
                .AddRedis(config.RedisConfig)
                .AddRedisMemoryStore();
        }
    }
}
