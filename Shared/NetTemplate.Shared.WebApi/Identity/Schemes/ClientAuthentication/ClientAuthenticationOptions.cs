using Microsoft.AspNetCore.Authentication;
using NetTemplate.Shared.WebApi.Identity.Models;

namespace NetTemplate.Shared.WebApi.Identity.Schemes.ClientAuthentication
{
    public class ClientAuthenticationOptions : AuthenticationSchemeOptions
    {
        public IEnumerable<ApplicationClient> Clients { get; set; }
    }
}
