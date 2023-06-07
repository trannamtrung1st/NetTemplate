using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace NetTemplate.Shared.Infrastructure.PubSub.Models
{
    public class PubSubConfig
    {
        public bool Enabled { get; set; }
        public IEnumerable<TopicSpecification> Topics { get; set; }
        public AdminClientConfig AdminConfig { get; set; }
        public CompetingConsumerConfig CommonConsumerConfig { get; set; }

        public static string GetSection(string topicName) => $"{Constants.ConfigurationSection}:{topicName}ConsumerConfig";
    }
}
