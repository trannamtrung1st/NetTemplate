using NetTemplate.Blog.Infrastructure.Common.Interfaces;
using NetTemplate.Common.DependencyInjection;
using NetTemplate.Shared.Infrastructure.PubSub.Models;

namespace NetTemplate.Blog.Infrastructure.Common.Implementations
{
    [ScopedService]
    public class ConsumerManager : IConsumerManager
    {
        public Task StartConsumers(PubSubConfig pubSubConfig, CancellationToken cancellationToken = default)
        {
            // [TODO]

            return Task.CompletedTask;
        }
    }
}
