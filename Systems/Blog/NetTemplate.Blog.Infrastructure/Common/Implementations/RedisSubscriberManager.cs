using NetTemplate.Blog.Infrastructure.Common.Interfaces;
using NetTemplate.Blog.Infrastructure.Domains.User.Subscribers;

namespace NetTemplate.Blog.Infrastructure.Common.Implementations
{
    public class RedisSubscriberManager : ITopicListenerManager
    {
        private readonly ISyncNewUserSubscriber _syncNewUserSubscriber;

        public RedisSubscriberManager(ISyncNewUserSubscriber syncNewUserSubscriber)
        {
            _syncNewUserSubscriber = syncNewUserSubscriber;
        }

        public async Task StartListeners(CancellationToken cancellationToken = default)
        {
            if (_syncNewUserSubscriber.Enabled) await _syncNewUserSubscriber.Start(cancellationToken);
        }
    }
}
