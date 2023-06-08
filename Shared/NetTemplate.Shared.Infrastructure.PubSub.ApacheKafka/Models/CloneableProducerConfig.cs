using Confluent.Kafka;

namespace NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Models
{
    public class CloneableProducerConfig : ProducerConfig, ICloneable
    {
        public CloneableProducerConfig()
        {
        }

        public CloneableProducerConfig(ClientConfig config) : base(config)
        {
        }

        public CloneableProducerConfig(IDictionary<string, string> config) : base(config)
        {
        }

        public object Clone()
        {
            Dictionary<string, string> config = this.ToDictionary(e => e.Key, e => e.Value);

            return new CloneableProducerConfig(config);
        }
    }
}
