using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NetTemplate.Shared.ClientSDK.Common.Handlers;
using NetTemplate.Shared.ClientSDK.Common.Models;

namespace NetTemplate.Shared.ClientSDK.Common.Extensions
{
    internal static class HttpClientBuilderExtensions
    {
        public static IHttpClientBuilder AddClientCredentials(this IHttpClientBuilder builder,
            string scope)
        {
            return builder
                .AddHttpMessageHandler((provider) =>
                {
                    var config = provider.GetRequiredService<IOptions<ClientConfig>>();
                    var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
                    var httpClient = httpClientFactory.CreateClient(nameof(ClientCredentialsTokenRequestHandler));
                    var options = new ClientCredentialsOptions()
                    {
                        ClientId = config.Value.ClientId,
                        ClientSecret = config.Value.ClientSecret,
                        Scope = scope
                    };
                    return new ClientCredentialsTokenRequestHandler(options, httpClient);
                });
        }
    }
}