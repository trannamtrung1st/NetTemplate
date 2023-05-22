using NetTemplate.Shared.ApplicationCore.Identity.Interfaces;
using System.Security.Claims;

namespace NetTemplate.Shared.ApplicationCore.Identity.Implementations
{
    public class NullCurrentUserProvider : ICurrentUserProvider
    {
        public ClaimsPrincipal User { get; }
        public string UserCode { get; }
        public int? UserId { get; }
    }
}
