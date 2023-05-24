namespace NetTemplate.Shared.WebApi.Models
{
    public class JwtConfig
    {
        public string Secret { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int TokenLifeTimeInSeconds { get; set; }

        private JwtConfig() { }

        private static JwtConfig _instance;
        public static JwtConfig Instance
        {
            get
            {
                if (_instance == null) _instance = new JwtConfig();
                return _instance;
            }
        }
    }
}
