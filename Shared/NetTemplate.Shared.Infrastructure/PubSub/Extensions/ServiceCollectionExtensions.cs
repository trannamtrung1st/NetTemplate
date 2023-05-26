using Microsoft.Extensions.DependencyInjection;
using NetTemplate.Shared.Infrastructure.PubSub.Models;

namespace NetTemplate.Shared.Infrastructure.PubSub.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPubSubIntegration(this IServiceCollection services, PubSubConfig pubSubConfig)
        {
            // [TODO]

            return services;
        }
    }
}
