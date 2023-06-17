using NetTemplate.Common.Objects.Interfaces;

namespace NetTemplate.Redis.Models
{
    public class RedisPubSubConfig : ICopyable<RedisPubSubConfig>
    {
        public IEnumerable<string> Subscribers { get; set; }

        public void CopyTo(RedisPubSubConfig other)
        {
            other.Subscribers = Subscribers;
        }

        public const string ConfigurationSection = nameof(RedisPubSubConfig);
    }
}
