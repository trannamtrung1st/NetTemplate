using NetTemplate.Common.Objects.Interfaces;

namespace NetTemplate.Shared.WebApi.Identity.Models
{
    public class SimulatedAuthConfig : ICopyable<SimulatedAuthConfig>
    {
        public bool Enabled { get; set; }
        public int UserId { get; set; }
        public string UserCode { get; set; }
        public IDictionary<string, string[]> Claims { get; set; }

        public void CopyTo(SimulatedAuthConfig other)
        {
            other.Enabled = Enabled;
            other.UserId = UserId;
            other.UserCode = UserCode;
            other.Claims = Claims;
        }


        public const string ConfigurationSection = nameof(SimulatedAuthConfig);
    }
}
