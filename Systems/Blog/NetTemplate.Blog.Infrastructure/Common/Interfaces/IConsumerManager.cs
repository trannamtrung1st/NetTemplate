using NetTemplate.Shared.Infrastructure.PubSub.Models;

namespace NetTemplate.Blog.Infrastructure.Common.Interfaces
{
    public interface IConsumerManager
    {
        Task StartConsumers(PubSubConfig pubSubConfig, CancellationToken cancellationToken = default);
    }
}
