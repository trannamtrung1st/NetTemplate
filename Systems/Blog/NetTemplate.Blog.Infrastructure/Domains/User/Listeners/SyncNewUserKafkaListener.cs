using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetTemplate.ApacheKafka.Implementations;
using NetTemplate.ApacheKafka.Interfaces;
using NetTemplate.ApacheKafka.Models;
using NetTemplate.Blog.ApplicationCore.User.Commands.SyncNewUser;
using NetTemplate.Blog.Infrastructure.Domains.User.Interfaces;
using NetTemplate.Shared.ApplicationCore.Common.Exceptions;
using NetTemplate.Shared.ApplicationCore.Domains.Identity.Models;
using ListenerNames = NetTemplate.Blog.Infrastructure.Domains.User.Constants.ListenerNames;
using TopicNames = NetTemplate.Shared.ApplicationCore.Domains.Identity.Constants.TopicNames;

namespace NetTemplate.Blog.Infrastructure.Domains.User.Listeners
{
    public class SyncNewUserKafkaListener
        : CompetingThreadConsumer<SyncNewUserKafkaListener, string, IdentityUserCreatedEventModel>
        , ISyncNewUserListener
    {
        public SyncNewUserKafkaListener(
            IServiceProvider provider,
            IExternalOffsetStore externalOffsetStore,
            IConfiguration configuration,
            IOptions<ApacheKafkaConfig> kafkaOptions,
            ILogger<SyncNewUserKafkaListener> logger)
            : base(provider, externalOffsetStore, configuration, kafkaOptions, logger)
        {
        }

        protected override string[] Topics => new[] { TopicNames.IdentityUserCreated };
        protected override string ConsumerName => ListenerNames.SyncNewUser;

        protected override async Task Handle(string topic, string key, IdentityUserCreatedEventModel value, IServiceProvider provider, CancellationToken cancellationToken = default)
        {
            IMediator mediator = provider.GetRequiredService<IMediator>();

            try
            {
                SyncNewUserCommand command = new SyncNewUserCommand(value.Model);

                await mediator.Send(command, cancellationToken);
            }
            catch (BaseException ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }
    }
}
