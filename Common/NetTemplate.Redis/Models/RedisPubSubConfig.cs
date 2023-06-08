using NetTemplate.Common.Objects;

namespace NetTemplate.Redis.Models
{
    public class RedisPubSubConfig : ICopyable<RedisPubSubConfig>
    {
        public IEnumerable<string> Subscribers { get; set; }

        public void CopyTo(RedisPubSubConfig other)
        {
            other.Subscribers = Subscribers;
        }
    }
}
