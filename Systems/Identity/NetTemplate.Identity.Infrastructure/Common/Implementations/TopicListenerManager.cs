using NetTemplate.Shared.Infrastructure.PubSub.Interfaces;

namespace NetTemplate.Identity.Infrastructure.Common.Implementations
{
    public class TopicListenerManager : ITopicListenerManager
    {
        public TopicListenerManager()
        {
        }

        public Task StartListeners(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
