using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetTemplate.Blog.ApplicationCore.Integrations.Identity;
using NetTemplate.Common.DependencyInjection;
using NetTemplate.Shared.ApplicationCore.Domains.Identity.Models;
using NetTemplate.Shared.Infrastructure.PubSub.Consumers;
using NetTemplate.Shared.Infrastructure.PubSub.Models;
using ConfigurationSections = NetTemplate.Blog.Infrastructure.Domains.Integrations.Identity.Constants.ConfigurationSections;
using TopicNames = NetTemplate.Blog.Infrastructure.Domains.Integrations.Identity.Constants.TopicNames;

namespace NetTemplate.Blog.Infrastructure.Domains.Integrations.Identity.Consumers
{
    public interface IIdentityUserCreatedConsumer
    {
        Task Start(CompetingConsumerConfig commonConfig, CancellationToken cancellationToken = default);
    }

    [ScopedService]
    public class IdentityUserCreatedConsumer
        : BaseConsumer<IdentityUserCreatedConsumer, string, IdentityUserCreatedEventModel>
        , IIdentityUserCreatedConsumer
    {
        public IdentityUserCreatedConsumer(IServiceProvider provider, IConfiguration configuration, ILogger<IdentityUserCreatedConsumer> logger)
            : base(provider, configuration, logger)
        {
        }

        protected override string TopicName => TopicNames.IdentityUserCreated;

        protected override CompetingConsumerConfig GetConfig(CompetingConsumerConfig commonConfig)
        {
            CompetingConsumerConfig consumerConfig = (CompetingConsumerConfig)commonConfig.Clone();

            configuration.GetSection(ConfigurationSections.IdentityUserCreated).Bind(consumerConfig);

            return consumerConfig;
        }

        protected override async Task Handle(IdentityUserCreatedEventModel model, IServiceProvider provider, CancellationToken cancellationToken = default)
        {
            IMediator mediator = provider.GetRequiredService<IMediator>();

            IdentityUserCreatedEvent @event = new IdentityUserCreatedEvent(model);

            await mediator.Send(@event, cancellationToken);
        }
    }
}
