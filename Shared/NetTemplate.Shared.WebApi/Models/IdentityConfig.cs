namespace NetTemplate.Shared.WebApi.Models
{
    public class IdentityConfig
    {
        public string ServerUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        private IdentityConfig() { }

        private static IdentityConfig _instance;
        public static IdentityConfig Instance
        {
            get
            {
                if (_instance == null) _instance = new IdentityConfig();
                return _instance;
            }
        }
    }
}
