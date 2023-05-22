using IdentityModel.Client;
using System.Net.Http.Headers;
using static IdentityModel.OidcConstants;

namespace NetTemplate.Shared.ClientSDK.Common.Handlers
{
    public class ClientCredentialsOptions
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
    }

    public class ClientCredentialsTokenRequestHandler : DelegatingHandler
    {
        private readonly ClientCredentialsOptions _options;
        private readonly HttpClient _httpClient;

        public ClientCredentialsTokenRequestHandler(ClientCredentialsOptions options,
            HttpClient httpClient)
        {
            _options = options;
            _httpClient = httpClient;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            const string TokenEndpoint = "/connect/token";
            var tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest
                {
                    Address = TokenEndpoint,
                    ClientId = _options.ClientId,
                    ClientSecret = _options.ClientSecret,
                    Scope = _options.Scope
                }, cancellationToken);


            if (tokenResponse.IsError)
            {
                throw tokenResponse.Exception;
            }

            request.Headers.Authorization = new AuthenticationHeaderValue(
                AuthenticationSchemes.AuthorizationHeaderBearer, tokenResponse.AccessToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
