using NetTemplate.Shared.Infrastructure.PubSub.Interfaces;

namespace NetTemplate.Blog.Infrastructure.Common.Implementations
{
    public class NullTopicManager : ITopicManager
    {
        public Task UpdateTopics(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
