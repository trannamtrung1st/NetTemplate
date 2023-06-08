using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Options;
using NetTemplate.Blog.Infrastructure.Common.Interfaces;
using NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Models;

namespace NetTemplate.Blog.Infrastructure.Common.Implementations
{
    public class KafkaTopicManager : ITopicManager
    {
        private readonly IAdminClient _adminClient;
        private readonly IOptions<ApacheKafkaConfig> _kafkaOptions;

        public KafkaTopicManager(IAdminClient adminClient,
            IOptions<ApacheKafkaConfig> kafkaOptions)
        {
            _adminClient = adminClient;
            _kafkaOptions = kafkaOptions;
        }

        public async Task UpdateTopics(CancellationToken cancellationToken = default)
        {
            ApacheKafkaConfig kafkaConfig = _kafkaOptions.Value;

            IEnumerable<TopicSpecification> configuredTopics = kafkaConfig.Topics ?? new TopicSpecification[0];

            Metadata metadata = _adminClient.GetMetadata(TimeSpan.FromSeconds(10));

            string[] topicNames = metadata.Topics.Select(t => t.Topic).ToArray();

            TopicSpecification[] newTopicSpecs = configuredTopics.Where(t => !topicNames.Contains(t.Name)).ToArray();

            string[] removedTopics = topicNames.Where(currentTopic => !configuredTopics.Any(t => t.Name == currentTopic)).ToArray();

            if (newTopicSpecs.Length > 0)
            {
                await _adminClient.CreateTopicsAsync(newTopicSpecs);
            }

            if (removedTopics.Length > 0)
            {
                await _adminClient.DeleteTopicsAsync(removedTopics);
            }
        }
    }
}
