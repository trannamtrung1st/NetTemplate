using Microsoft.Extensions.DependencyInjection;
using NetTemplate.Shared.ApplicationCore.Identity.Implementations;
using NetTemplate.Shared.ApplicationCore.Identity.Interfaces;

namespace NetTemplate.Shared.ApplicationCore.Identity.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNullCurrentUserProvider(this IServiceCollection services)
        {
            return services.AddSingleton<ICurrentUserProvider, NullCurrentUserProvider>();
        }
    }
}
