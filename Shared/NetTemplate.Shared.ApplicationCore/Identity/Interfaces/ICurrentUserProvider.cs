using System.Security.Claims;

namespace NetTemplate.Shared.ApplicationCore.Identity.Interfaces
{
    public interface ICurrentUserProvider
    {
        ClaimsPrincipal User { get; }
        string UserCode { get; }
        int? UserId { get; }
    }
}
