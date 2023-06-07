using Confluent.Kafka;
using Confluent.Kafka.Admin;
using NetTemplate.Blog.Infrastructure.Common.Interfaces;
using NetTemplate.Common.DependencyInjection;
using NetTemplate.Shared.Infrastructure.PubSub.Models;

namespace NetTemplate.Blog.Infrastructure.Common.Implementations
{
    [ScopedService]
    public class TopicManager : ITopicManager
    {
        private readonly IAdminClient _adminClient;

        public TopicManager(IAdminClient adminClient)
        {
            _adminClient = adminClient;
        }

        public async Task UpdateTopics(PubSubConfig pubSubConfig, CancellationToken cancellationToken = default)
        {
            if (!pubSubConfig.Enabled) return;

            IEnumerable<TopicSpecification> configuredTopics = pubSubConfig.Topics ?? new TopicSpecification[0];

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
