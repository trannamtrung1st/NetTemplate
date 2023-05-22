using NetTemplate.Shared.ApplicationCore.Domains.Identity.Models;

namespace NetTemplate.Shared.ClientSDK.Identity
{
    public interface IIdentityClient
    {
        Task<IEnumerable<IdentityUserModel>> GetUsers(CancellationToken cancellationToken = default);
    }
}
