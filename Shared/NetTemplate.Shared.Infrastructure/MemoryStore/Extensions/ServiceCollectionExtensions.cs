using Microsoft.Extensions.DependencyInjection;
using NetTemplate.Common.MemoryStore.Interfaces;
using NetTemplate.Shared.Infrastructure.MemoryStore.Implementations;

namespace NetTemplate.Shared.Infrastructure.MemoryStore.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRedisMemoryStore(this IServiceCollection services)
        {
            return services.AddSingleton<IMemoryStore, RedisMemoryStore>();
        }
    }
}
