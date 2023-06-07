using Confluent.Kafka;

namespace NetTemplate.Shared.Infrastructure.PubSub.Models
{
    public class CompetingConsumerConfig : ConsumerConfig, ICloneable
    {
        public int DefaultRetryAfter { get; set; }
        public int ConsumerCount { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
