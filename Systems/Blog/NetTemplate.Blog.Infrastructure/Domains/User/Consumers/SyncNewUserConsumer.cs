﻿using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetTemplate.ApacheKafka.Implementations;
using NetTemplate.ApacheKafka.Interfaces;
using NetTemplate.ApacheKafka.Models;
using NetTemplate.Blog.ApplicationCore.User.Commands.SyncNewUser;
using NetTemplate.Common.DependencyInjection;
using NetTemplate.Shared.ApplicationCore.Domains.Identity.Models;
using ListenerNames = NetTemplate.Blog.Infrastructure.Domains.User.Constants.ListenerNames;
using TopicNames = NetTemplate.Blog.Infrastructure.Integrations.Identity.Constants.TopicNames;

namespace NetTemplate.Blog.Infrastructure.Domains.User.Consumers
{
    public interface ISyncNewUserConsumer : IGeneralConsumer
    {
    }

    [SingletonService]
    public class SyncNewUserConsumer
        : CompetingThreadConsumer<SyncNewUserConsumer, string, IdentityUserCreatedEventModel>
        , ISyncNewUserConsumer
    {
        public SyncNewUserConsumer(
            IServiceProvider provider,
            IExternalOffsetStore externalOffsetStore,
            IConfiguration configuration,
            IOptions<ApacheKafkaConfig> kafkaOptions,
            ILogger<SyncNewUserConsumer> logger)
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
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }
    }
}
