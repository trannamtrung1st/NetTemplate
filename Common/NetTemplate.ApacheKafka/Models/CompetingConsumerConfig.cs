using Confluent.Kafka;

namespace NetTemplate.ApacheKafka.Models
{
    public class CompetingConsumerConfig : ConsumerConfig, ICloneable
    {
        public CompetingConsumerConfig()
        {
        }

        public CompetingConsumerConfig(ClientConfig config) : base(config)
        {
        }

        public CompetingConsumerConfig(IDictionary<string, string> config) : base(config)
        {
        }

        public bool UseExternalOffsetStore { get; set; }
        public int DefaultRetryAfter { get; set; }
        public int ConsumerCount { get; set; }
        public int MaxRetryCount { get; set; }

        public object Clone()
        {
            Dictionary<string, string> config = this.ToDictionary(e => e.Key, e => e.Value);

            return new CompetingConsumerConfig(config)
            {
                UseExternalOffsetStore = UseExternalOffsetStore,
                DefaultRetryAfter = DefaultRetryAfter,
                ConsumerCount = ConsumerCount,
                MaxRetryCount = MaxRetryCount
            };
        }
    }
}
