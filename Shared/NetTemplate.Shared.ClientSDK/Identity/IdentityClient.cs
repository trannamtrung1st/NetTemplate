using Microsoft.AspNetCore.Http;
using NetTemplate.Shared.ApplicationCore.Domains.Identity.Models;
using Newtonsoft.Json;

namespace NetTemplate.Shared.ClientSDK.Identity
{
    public interface IIdentityClient
    {
        Task<IEnumerable<IdentityUserModel>> GetUsers(CancellationToken cancellationToken = default);
    }

    public class IdentityClient : IIdentityClient
    {
        private readonly HttpClient _client;

        public IdentityClient(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient(Constants.ClientName);
        }

        public async Task<IEnumerable<IdentityUserModel>> GetUsers(CancellationToken cancellationToken = default)
        {
            var apiEndpoint = new PathString(Constants.ApiEndpoints.User.GetUsers);
            var response = await _client.GetAsync(apiEndpoint, cancellationToken);
            response = response.EnsureSuccessStatusCode();

            var apiResponseStr = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<IEnumerable<IdentityUserModel>>(apiResponseStr);
            return users;
        }
    }
}