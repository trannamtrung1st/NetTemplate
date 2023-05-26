using Microsoft.Extensions.DependencyInjection;
using NetTemplate.Shared.Infrastructure.Resilience.Utils;
using Polly.Registry;

namespace NetTemplate.Shared.Infrastructure.Resilience.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddResilience(this IServiceCollection services)
        {
            return services.AddSingleton<IReadOnlyPolicyRegistry<string>, PolicyRegistry>(
                (serviceProvider) => PollyHelper.InitPolly(serviceProvider));
        }
    }
}
