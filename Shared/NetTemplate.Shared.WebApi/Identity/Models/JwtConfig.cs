using NetTemplate.Common.Objects;

namespace NetTemplate.Shared.WebApi.Identity.Models
{
    public class JwtConfig : ICopyable<JwtConfig>
    {
        public string Secret { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int TokenLifeTimeInSeconds { get; set; }

        public void CopyTo(JwtConfig other)
        {
            other.Secret = Secret;
            other.Audience = Audience;
            other.Issuer = Issuer;
            other.TokenLifeTimeInSeconds = TokenLifeTimeInSeconds;
        }
    }
}
