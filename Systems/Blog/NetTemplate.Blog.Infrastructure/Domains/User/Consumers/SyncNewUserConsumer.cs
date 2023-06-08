using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetTemplate.Blog.ApplicationCore.User.Commands.SyncNewUser;
using NetTemplate.Common.DependencyInjection;
using NetTemplate.Shared.ApplicationCore.Domains.Identity.Models;
using NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Extensions;
using NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Implementations;
using NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Interfaces;
using NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Models;
using ConsumerNames = NetTemplate.Blog.Infrastructure.Domains.User.Constants.ConsumerNames;
using TopicNames = NetTemplate.Blog.Infrastructure.Integrations.Identity.Constants.TopicNames;

namespace NetTemplate.Blog.Infrastructure.Domains.User.Consumers
{
    public interface ISyncNewUserConsumer : IGeneralConsumer
    {
    }

    [ScopedService]
    public class SyncNewUserConsumer
        : CompetingThreadConsumer<SyncNewUserConsumer, string, IdentityUserCreatedEventModel>
        , ISyncNewUserConsumer
    {
        public SyncNewUserConsumer(
            IServiceProvider provider,
            IOffsetStore offsetStore,
            IConfiguration configuration,
            ILogger<SyncNewUserConsumer> logger)
            : base(provider, offsetStore, configuration, logger)
        {
        }

        protected override string[] Topics => new[] { TopicNames.IdentityUserCreated };

        protected override CompetingConsumerConfig GetConfig(CompetingConsumerConfig commonConfig)
        {
            CompetingConsumerConfig consumerConfig = (CompetingConsumerConfig)commonConfig.Clone();

            configuration.BindConsumerConfig(ConsumerNames.SyncNewUser, consumerConfig);

            return consumerConfig;
        }

        protected override async Task Handle(string topic, string key, IdentityUserCreatedEventModel value, IServiceProvider provider, CancellationToken cancellationToken = default)
        {
            IMediator mediator = provider.GetRequiredService<IMediator>();

            try
            {
                SyncNewUserCommand command = new SyncNewUserCommand(value.Model);

                await mediator.Send(command, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }
    }
}
