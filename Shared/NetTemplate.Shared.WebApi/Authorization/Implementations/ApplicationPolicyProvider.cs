using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace NetTemplate.Shared.WebApi.Authorization.Implementations
{
    // [IMPORTANT] process policy names by syntax: policyA[,args];policyB[,args]...
    // So we don't have to create specific policy for each endpoint, just use the syntax with generic policies defined
    // Or we can change the requirements based on configuration in database, etc.
    public class ApplicationPolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly DefaultAuthorizationPolicyProvider _defaultPolicyProvider;

        public ApplicationPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            // ASP.NET Core only uses one authorization policy provider, so if the custom implementation
            // doesn't handle all policies it should fall back to an alternate provider.
            _defaultPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => _defaultPolicyProvider.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy> GetFallbackPolicyAsync() => _defaultPolicyProvider.GetFallbackPolicyAsync();

        public async Task<AuthorizationPolicy> GetPolicyAsync(string policyId)
        {
            AuthorizationPolicy policy = await _defaultPolicyProvider.GetPolicyAsync(policyId);

            return policy;
        }
    }
}
