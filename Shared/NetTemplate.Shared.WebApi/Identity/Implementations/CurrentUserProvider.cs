using NetTemplate.Common.DependencyInjection;
using NetTemplate.Shared.ApplicationCore.Identity.Interfaces;
using System.Security.Claims;

namespace NetTemplate.Shared.WebApi.Identity.Implementations
{
    [ScopedService]
    public class CurrentUserProvider : ICurrentUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;
        public string UserCode => User?.Identity?.Name;
        public int? UserId => int.TryParse(User?.FindFirstValue(ClaimTypes.NameIdentifier), out int id)
            ? id : default;
    }
}
