using Microsoft.Extensions.DependencyInjection;
using NetTemplate.Common.Synchronization.Implementations;
using NetTemplate.Common.Synchronization.Interfaces;

namespace NetTemplate.Common.Synchronization.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSemaphoreSlimLock(this IServiceCollection services)
        {
            return services.AddSingleton<IDistributedLock, SemaphoreSlimLock>();
        }
    }
}
