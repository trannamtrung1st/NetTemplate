using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;

namespace NetTemplate.Shared.WebApi.Identity.Schemes.SimulatedAuthentication
{
    public class SimulatedAuthenticationHandler : AuthenticationHandler<SimulatedAuthenticationOptions>
    {
        public const string AuthorizationScheme = "Bearer";
        public const string WWWAuthenticateValue = "Bearer";

        public SimulatedAuthenticationHandler(IOptionsMonitor<SimulatedAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.Headers.Add(HeaderNames.WWWAuthenticate, WWWAuthenticateValue);
            return base.HandleChallengeAsync(properties);
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!string.IsNullOrWhiteSpace(Options.UserCode) && Options.UserId > 0)
            {
                IDictionary<string, string[]> claims = Options.Claims;
                GenericIdentity identity = new GenericIdentity(Options.UserCode);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, Options.UserId.ToString()));

                if (claims != null)
                {
                    foreach (KeyValuePair<string, string[]> kvp in claims)
                    {
                        Claim[] claimList = kvp.Value.Select(val => new Claim(kvp.Key, val)).ToArray();
                        identity.AddClaims(claimList);
                    }
                }

                GenericPrincipal principal = new GenericPrincipal(identity, null);
                AuthenticationTicket ticket = new AuthenticationTicket(principal, Scheme.Name);

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            return Task.FromResult(AuthenticateResult.NoResult());
        }
    }
}
