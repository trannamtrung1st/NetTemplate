using NetTemplate.Common.Objects;

namespace NetTemplate.Shared.ClientSDK.Common.Models
{
    public class ClientConfig : ICopyable<ClientConfig>
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string IdentityServerUrl { get; set; }

        public void CopyTo(ClientConfig other)
        {
            other.ClientId = ClientId;
            other.ClientSecret = ClientSecret;
            other.IdentityServerUrl = IdentityServerUrl;
        }


        public const string ConfigurationSection = nameof(ClientConfig);
    }
}
