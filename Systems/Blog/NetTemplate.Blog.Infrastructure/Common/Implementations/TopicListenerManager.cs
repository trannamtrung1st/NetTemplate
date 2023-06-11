using NetTemplate.Blog.Infrastructure.Domains.User.Interfaces;
using NetTemplate.Shared.Infrastructure.PubSub.Interfaces;

namespace NetTemplate.Blog.Infrastructure.Common.Implementations
{
    public class TopicListenerManager : ITopicListenerManager
    {
        private readonly ISyncNewUserListener _syncNewUserListener;

        public TopicListenerManager(ISyncNewUserListener syncNewUserListener)
        {
            _syncNewUserListener = syncNewUserListener;
        }

        public async Task StartListeners(CancellationToken cancellationToken = default)
        {
            if (_syncNewUserListener.Enabled) await _syncNewUserListener.Start(cancellationToken);
        }
    }
}
