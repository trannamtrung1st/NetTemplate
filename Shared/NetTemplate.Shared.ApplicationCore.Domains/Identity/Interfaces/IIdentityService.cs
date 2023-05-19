using NetTemplate.Shared.ApplicationCore.Domains.Identity.Models;

namespace NetTemplate.Shared.ApplicationCore.Domains.Identity.Interfaces
{
    public interface IIdentityService
    {
        Task<IEnumerable<IdentityUserModel>> GetIdentityUsers(CancellationToken cancellationToken = default);
    }
}
