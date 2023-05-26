using NetTemplate.Shared.ApplicationCore.Identity.Interfaces;
using System.Security.Claims;

namespace NetTemplate.Shared.WebApi.Identity.Implementations
{
    public class RequestCurrentUserProvider : ICurrentUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequestCurrentUserProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;
        public string UserCode => User?.Identity?.Name;
        public int? UserId => int.TryParse(User?.FindFirstValue(ClaimTypes.NameIdentifier), out int id)
            ? id : default;
    }
}
