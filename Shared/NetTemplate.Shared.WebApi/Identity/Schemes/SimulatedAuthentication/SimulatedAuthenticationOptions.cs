using Microsoft.AspNetCore.Authentication;

namespace NetTemplate.Shared.WebApi.Identity.Schemes.SimulatedAuthentication
{
    public class SimulatedAuthenticationOptions : AuthenticationSchemeOptions
    {
        public int UserId { get; set; }
        public string UserCode { get; set; }
        public IDictionary<string, string[]> Claims { get; set; }
    }
}
