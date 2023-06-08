using Microsoft.Extensions.Options;
using NetTemplate.Blog.Infrastructure.Common.Interfaces;
using NetTemplate.Blog.Infrastructure.Domains.User.Consumers;
using NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Models;

namespace NetTemplate.Blog.Infrastructure.Common.Implementations
{
    public class KafkaConsumerManager : ITopicListenerManager
    {
        private readonly ISyncNewUserConsumer _syncNewUserConsumer;
        private readonly IOptions<ApacheKafkaConfig> _kafkaOptions;

        public KafkaConsumerManager(
            ISyncNewUserConsumer syncNewUserConsumer,
            IOptions<ApacheKafkaConfig> kafkaOptions)
        {
            _syncNewUserConsumer = syncNewUserConsumer;
            _kafkaOptions = kafkaOptions;
        }

        public async Task StartListeners(CancellationToken cancellationToken = default)
        {
            ApacheKafkaConfig kafkaConfig = _kafkaOptions.Value;

            await _syncNewUserConsumer.Start(kafkaConfig.CommonConsumerConfig, cancellationToken);
        }

    }
}
