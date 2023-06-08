﻿using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetTemplate.Blog.ApplicationCore.User.Commands.SyncNewUser;
using NetTemplate.Common.DependencyInjection;
using NetTemplate.Redis.Implementations;
using NetTemplate.Redis.Interfaces;
using NetTemplate.Redis.Models;
using NetTemplate.Shared.ApplicationCore.Domains.Identity.Models;
using Newtonsoft.Json;
using StackExchange.Redis;
using ListenerNames = NetTemplate.Blog.Infrastructure.Domains.User.Constants.ListenerNames;
using TopicNames = NetTemplate.Blog.Infrastructure.Integrations.Identity.Constants.TopicNames;

namespace NetTemplate.Blog.Infrastructure.Domains.User.Subscribers
{
    public interface ISyncNewUserSubscriber : IGeneralSubscriber
    {
    }

    [SingletonService]
    public class SyncNewUserSubscriber : BaseSubscriber<SyncNewUserSubscriber>, ISyncNewUserSubscriber
    {
        public SyncNewUserSubscriber(
            IServiceProvider provider,
            ConnectionMultiplexer connectionMultiplexer,
            IOptions<RedisPubSubConfig> pubSubOptions,
            ILogger<SyncNewUserSubscriber> logger)
            : base(provider, connectionMultiplexer, pubSubOptions, logger)
        {
        }

        protected override RedisChannel Channel => TopicNames.IdentityUserCreated;

        protected override string SubcriberName => ListenerNames.SyncNewUser;

        public override async Task Handle(RedisChannel channel, RedisValue value)
        {
            IMediator mediator = provider.GetRequiredService<IMediator>();

            try
            {
                IdentityUserCreatedEventModel @event = JsonConvert.DeserializeObject<IdentityUserCreatedEventModel>(value);

                SyncNewUserCommand command = new SyncNewUserCommand(@event.Model);

                await mediator.Send(command, default);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }
    }
}
