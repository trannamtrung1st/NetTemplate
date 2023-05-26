namespace NetTemplate.Shared.WebApi.Identity.Models
{
    public class JwtConfig
    {
        public string Secret { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int TokenLifeTimeInSeconds { get; set; }
    }
}
