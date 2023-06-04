using Microsoft.Extensions.DependencyInjection;
using NetTemplate.Shared.ApplicationCore.Domains.Identity.Interfaces;
using NetTemplate.Shared.ClientSDK.Identity.Extensions;
using NetTemplate.Shared.Infrastructure.Domains.Identity.Implementations;
using NetTemplate.Shared.Infrastructure.Resilience.Utils;

namespace NetTemplate.Shared.Infrastructure.Domains.Identity.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection services)
        {
            IHttpClientBuilder httpBuilder = services.AddIdentityClient();

            httpBuilder.AddPolicyHandlerFromRegistry(PollyHelper.TransientHttpErrorPolicy);

            return services.AddScoped<IIdentityService, IdentityService>();
        }
    }
}
