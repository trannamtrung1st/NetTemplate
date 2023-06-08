using Confluent.Kafka;
using Confluent.Kafka.Admin;
using NetTemplate.Common.Objects;

namespace NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Models
{
    public class ApacheKafkaConfig : ICopyable<ApacheKafkaConfig>
    {
        public bool Enabled { get; set; }
        public IEnumerable<TopicSpecification> Topics { get; set; }
        public AdminClientConfig AdminConfig { get; set; }
        public CompetingConsumerConfig CommonConsumerConfig { get; set; }
        public CloneableProducerConfig CommonProducerConfig { get; set; }

        public void CopyTo(ApacheKafkaConfig other)
        {
            other.Enabled = Enabled;
            other.Topics = Topics;
            other.AdminConfig = AdminConfig;
            other.CommonConsumerConfig = CommonConsumerConfig;
            other.CommonProducerConfig = CommonProducerConfig;
        }
    }
}
