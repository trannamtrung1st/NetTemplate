using NetTemplate.Shared.Infrastructure.PubSub.Models;

namespace NetTemplate.Blog.Infrastructure.Common.Interfaces
{
    public interface ITopicManager
    {
        Task UpdateTopics(PubSubConfig pubSubConfig, CancellationToken cancellationToken = default);
    }
}
