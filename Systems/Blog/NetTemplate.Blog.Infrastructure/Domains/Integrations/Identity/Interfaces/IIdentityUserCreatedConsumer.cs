using NetTemplate.Shared.Infrastructure.PubSub.Models;

namespace NetTemplate.Blog.Infrastructure.Domains.Integrations.Identity.Interfaces
{
    public interface IIdentityUserCreatedConsumer
    {
        Task Start(CompetingConsumerConfig commonConfig, CancellationToken cancellationToken = default);
    }
}
