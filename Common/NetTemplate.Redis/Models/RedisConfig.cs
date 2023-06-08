using NetTemplate.Common.Objects;

namespace NetTemplate.Redis.Models
{
    public class RedisConfig : ICopyable<RedisConfig>
    {
        public bool Enabled { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public IEnumerable<string> Endpoints { get; set; }

        public void CopyTo(RedisConfig other)
        {
            other.Enabled = Enabled;
            other.User = User;
            other.Password = Password;
            other.Endpoints = Endpoints;
        }
    }
}
