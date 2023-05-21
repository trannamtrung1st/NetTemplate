using System.Security.Claims;

namespace NetTemplate.Shared.ApplicationCore.Interfaces
{
    public interface ICurrentUserProvider
    {
        ClaimsPrincipal User { get; }
        string UserCode { get; }
        int? UserId { get; }
    }

    public class NullCurrentUserProvider : ICurrentUserProvider
    {
        public ClaimsPrincipal User { get; }
        public string UserCode { get; }
        public int? UserId { get; }
    }
}
