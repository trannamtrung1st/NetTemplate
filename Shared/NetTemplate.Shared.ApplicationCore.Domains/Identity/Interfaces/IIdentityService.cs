using NetTemplate.Shared.ApplicationCore.Common.Interfaces;
using NetTemplate.Shared.ApplicationCore.Domains.Identity.Models;

namespace NetTemplate.Shared.ApplicationCore.Domains.Identity.Interfaces
{
    public interface IIdentityService : IDomainService
    {
        Task<IEnumerable<IdentityUserModel>> GetIdentityUsers(CancellationToken cancellationToken = default);
    }
}
