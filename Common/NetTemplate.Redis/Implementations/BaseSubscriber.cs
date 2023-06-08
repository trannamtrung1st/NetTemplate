using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetTemplate.Redis.Interfaces;
using NetTemplate.Redis.Models;
using StackExchange.Redis;

namespace NetTemplate.Redis.Implementations
{
    public abstract class BaseSubscriber<T> : IGeneralSubscriber
    {
        protected readonly IServiceProvider provider;
        protected readonly ConnectionMultiplexer connectionMultiplexer;
        protected readonly IOptions<RedisPubSubConfig> pubSubOptions;
        protected readonly ILogger<T> logger;

        public BaseSubscriber(
            IServiceProvider provider,
            ConnectionMultiplexer connectionMultiplexer,
            IOptions<RedisPubSubConfig> pubSubOptions,
            ILogger<T> logger)
        {
            this.provider = provider;
            this.connectionMultiplexer = connectionMultiplexer;
            this.pubSubOptions = pubSubOptions;
            this.logger = logger;
        }

        private bool? _enabled;
        public bool Enabled
        {
            get
            {
                if (_enabled == null)
                {
                    _enabled = pubSubOptions.Value.Subscribers.Contains(SubcriberName);
                }

                return _enabled.Value;
            }
        }

        protected abstract RedisChannel Channel { get; }
        protected abstract string SubcriberName { get; }

        public async Task Start(CancellationToken cancellationToken = default)
        {
            await connectionMultiplexer.GetSubscriber().SubscribeAsync(Channel,
                handler: async (channel, value) => await Handle(channel, value));
        }

        public abstract Task Handle(RedisChannel channel, RedisValue value);
    }
}
