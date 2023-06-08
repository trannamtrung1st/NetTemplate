using NetTemplate.Blog.Infrastructure.Common.Interfaces;

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
