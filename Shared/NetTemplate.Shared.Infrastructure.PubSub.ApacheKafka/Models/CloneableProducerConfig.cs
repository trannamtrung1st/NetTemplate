using Confluent.Kafka;

namespace NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Models
{
    public class CloneableProducerConfig : ProducerConfig, ICloneable
    {
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
