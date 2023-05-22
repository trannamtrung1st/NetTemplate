using Microsoft.AspNetCore.Http;
using NetTemplate.Shared.ApplicationCore.Domains.Identity.Models;
using Newtonsoft.Json;

namespace NetTemplate.Shared.ClientSDK.Identity
{
    public class IdentityClient : IIdentityClient
    {
        private readonly HttpClient _client;

        public IdentityClient(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient(IdentitySDKConstants.ClientName);
        }

        public async Task<IEnumerable<IdentityUserModel>> GetUsers(CancellationToken cancellationToken = default)
        {
            var apiEndpoint = new PathString(IdentitySDKConstants.ApiEndpoints.User.GetUsers);
            var response = await _client.GetAsync(apiEndpoint, cancellationToken);
            response = response.EnsureSuccessStatusCode();

            var apiResponseStr = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<IEnumerable<IdentityUserModel>>(apiResponseStr);
            return users;
        }
    }
}