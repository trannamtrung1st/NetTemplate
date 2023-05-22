﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NetTemplate.Shared.ClientSDK.Common.Extensions;
using NetTemplate.Shared.ClientSDK.Common.Handlers;
using NetTemplate.Shared.ClientSDK.Common.Models;

namespace NetTemplate.Shared.ClientSDK.Identity.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IHttpClientBuilder AddIdentityClient(
            this IServiceCollection services)
        {
            services.AddSingleton<IIdentityClient, IdentityClient>();

            var identityClientBuilder = services.AddHttpClient(IdentitySDKConstants.ClientName, (provider, httpClient) =>
            {
                var config = provider.GetRequiredService<IOptions<ClientConfiguration>>();
                httpClient.BaseAddress = new Uri(config.Value.IdentityServerUrl);
            })
                .AddClientCredentials(IdentitySDKConstants.ApiScopes.Identity)
                .AddHttpMessageHandler<WrapHttpErrorResponseHandler>();

            return identityClientBuilder;
        }
    }
}
