using NetTemplate.Blog.Infrastructure.Common.Interfaces;
using NetTemplate.Blog.Infrastructure.Domains.Integrations.Identity.Interfaces;
using NetTemplate.Common.DependencyInjection;
using NetTemplate.Shared.Infrastructure.PubSub.Models;

namespace NetTemplate.Blog.Infrastructure.Common.Implementations
{
    [ScopedService]
    public class ConsumerManager : IConsumerManager
    {
        private readonly IIdentityUserCreatedConsumer _identityUserCreatedConsumer;

        public ConsumerManager(IIdentityUserCreatedConsumer identityUserCreatedConsumer)
        {
            _identityUserCreatedConsumer = identityUserCreatedConsumer;
        }

        public async Task StartConsumers(PubSubConfig pubSubConfig, CancellationToken cancellationToken = default)
        {
            await _identityUserCreatedConsumer.Start(pubSubConfig.CommonConsumerConfig, cancellationToken);
        }
    }
}
