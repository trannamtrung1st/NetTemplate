using Microsoft.Extensions.DependencyInjection;
using NetTemplate.Common.DependencyInjection.Extensions;
using NetTemplate.Shared.ClientSDK.Common.Handlers;
using NetTemplate.Shared.ClientSDK.Common.Models;

namespace NetTemplate.Shared.ClientSDK.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddClientSdkDefaults(this IServiceCollection services,
            ClientConfig config)
        {
            return services.AddScoped<WrapHttpErrorResponseHandler>()
                .ConfigureCopyableConfig(config);
        }

        public static IHttpClientBuilder AddTokenManagement(this IServiceCollection services,
            ClientConfig config)
        {
            if (config?.IdentityServerUrl == null) throw new ArgumentNullException(nameof(config));

            return services.AddHttpClient(nameof(ClientCredentialsTokenRequestHandler), httpClient =>
            {
                httpClient.BaseAddress = new Uri(config.IdentityServerUrl);
            });
        }
    }
}
