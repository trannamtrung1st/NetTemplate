using NetTemplate.Shared.Infrastructure.PubSub.Interfaces;

namespace NetTemplate.Shared.Infrastructure.PubSub.Implementations
{
    public class NullTopicManager : ITopicManager
    {
        public Task UpdateTopics(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
