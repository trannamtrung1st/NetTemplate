using Microsoft.Extensions.DependencyInjection;
using NetTemplate.Shared.ClientSDK.Common.Handlers;
using NetTemplate.Shared.ClientSDK.Common.Models;

namespace NetTemplate.Shared.ClientSDK.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddClientSdkHandlers(this IServiceCollection services)
        {
            return services.AddScoped<WrapHttpErrorResponseHandler>();
        }

        public static IHttpClientBuilder AddTokenManagement(this IServiceCollection services,
            ClientConfiguration config)
        {
            if (config?.IdentityServerUrl == null) throw new ArgumentNullException(nameof(config));

            services.Configure<ClientConfiguration>(opt =>
            {
                opt.ClientId = config.ClientId;
                opt.ClientSecret = config.ClientSecret;
                opt.IdentityServerUrl = config.IdentityServerUrl;
            });

            return services.AddHttpClient(nameof(ClientCredentialsTokenRequestHandler), httpClient =>
            {
                httpClient.BaseAddress = new Uri(config.IdentityServerUrl);
            });
        }
    }
}
