using Microsoft.Extensions.DependencyInjection;
using NetTemplate.Common.MemoryStore.Implementations;
using NetTemplate.Common.MemoryStore.Interfaces;

namespace NetTemplate.Common.MemoryStore.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSimpleMemoryStore(this IServiceCollection services)
        {
            return services.AddSingleton<IMemoryStore, SimpleMemoryStore>();
        }
    }
}
