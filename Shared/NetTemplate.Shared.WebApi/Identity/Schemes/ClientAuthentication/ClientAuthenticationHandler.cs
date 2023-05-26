using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Text.Encodings.Web;

namespace NetTemplate.Shared.WebApi.Identity.Schemes.ClientAuthentication
{
    public class ClientAuthenticationHandler : AuthenticationHandler<ClientAuthenticationOptions>
    {
        public const string AuthorizationScheme = "Basic";
        public const string WWWAuthenticateValue = "Client";

        public ClientAuthenticationHandler(IOptionsMonitor<ClientAuthenticationOptions> options,
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
            var clients = Options.Clients;
            if (clients?.Any() != true)
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            var request = Request;
            var authHeader = request.Headers[HeaderNames.Authorization];

            if (!string.IsNullOrWhiteSpace(authHeader))
            {
                var authHeaderVal = AuthenticationHeaderValue.Parse(authHeader);

                if (authHeaderVal.Scheme.Equals(AuthorizationScheme, StringComparison.OrdinalIgnoreCase) &&
                    !string.IsNullOrWhiteSpace(authHeaderVal.Parameter))
                {
                    const string IsoCharset = "ISO-8859-1";
                    var encoding = Encoding.GetEncoding(IsoCharset);
                    string credentials = encoding.GetString(Convert.FromBase64String(authHeaderVal.Parameter));

                    int separator = credentials.IndexOf(':');
                    string clientId = credentials.Substring(0, separator);
                    string clientSecret = credentials.Substring(separator + 1);
                    var currentClient = clients.FirstOrDefault(c => c.ClientId == clientId && c.ClientSecret == clientSecret);

                    if (currentClient != null)
                    {
                        var identity = new GenericIdentity(currentClient.Name);
                        var principal = new GenericPrincipal(identity, null);
                        var ticket = new AuthenticationTicket(principal, Scheme.Name);
                        return Task.FromResult(AuthenticateResult.Success(ticket));
                    }
                }
            }

            return Task.FromResult(AuthenticateResult.NoResult());
        }
    }
}
