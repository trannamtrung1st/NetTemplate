using Confluent.Kafka;
using Confluent.Kafka.Admin;
using NetTemplate.Common.Objects;

namespace NetTemplate.ApacheKafka.Models
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

        public const string ConfigurationSection = nameof(ApacheKafkaConfig);
        public const string ConsumersConfigurationSection = "ApacheKafkaConsumers";
        public const string ProducersConfigurationSection = "ApacheKafkaProducers";

        public static string GetConsumerSection(string name) => $"{ConsumersConfigurationSection}:{name}";
        public static string GetProducerSection(string name) => $"{ProducersConfigurationSection}:{name}";
    }
}
