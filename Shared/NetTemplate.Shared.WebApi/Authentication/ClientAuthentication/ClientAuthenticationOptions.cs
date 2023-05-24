using Microsoft.AspNetCore.Authentication;
using NetTemplate.Shared.WebApi.Models;

namespace NetTemplate.Shared.WebApi.Authentication.ClientAuthentication
{
    public class ClientAuthenticationOptions : AuthenticationSchemeOptions
    {
        public IEnumerable<ApplicationClient> Clients { get; set; }
    }
}
