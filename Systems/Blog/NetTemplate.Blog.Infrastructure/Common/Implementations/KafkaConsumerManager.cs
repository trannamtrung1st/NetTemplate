using NetTemplate.Blog.Infrastructure.Common.Interfaces;
using NetTemplate.Blog.Infrastructure.Domains.User.Consumers;

namespace NetTemplate.Blog.Infrastructure.Common.Implementations
{
    public class KafkaConsumerManager : ITopicListenerManager
    {
        private readonly ISyncNewUserConsumer _syncNewUserConsumer;

        public KafkaConsumerManager(
            ISyncNewUserConsumer syncNewUserConsumer)
        {
            _syncNewUserConsumer = syncNewUserConsumer;
        }

        public async Task StartListeners(CancellationToken cancellationToken = default)
        {
            if (_syncNewUserConsumer.Enabled) await _syncNewUserConsumer.Start(cancellationToken);
        }

    }
}
