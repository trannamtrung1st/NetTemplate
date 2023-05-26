using NetTemplate.Shared.ApplicationCore.Domains.Identity.Interfaces;
using NetTemplate.Shared.ApplicationCore.Domains.Identity.Models;
using NetTemplate.Shared.ClientSDK.Identity;

namespace NetTemplate.Shared.Infrastructure.Domains.Identity.Implementations
{
    public class IdentityService : IIdentityService
    {
        private readonly IIdentityClient _identityClient;

        public IdentityService(IIdentityClient identityClient)
        {
            _identityClient = identityClient;
        }

        public async Task<IEnumerable<IdentityUserModel>> GetIdentityUsers(CancellationToken cancellationToken = default)
        {
            IEnumerable<IdentityUserModel> identityUsers = await _identityClient.GetUsers(cancellationToken);

            return identityUsers;
        }
    }
}
