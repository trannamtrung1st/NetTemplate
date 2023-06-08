using Microsoft.Extensions.Configuration;
using NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Models;

namespace NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Extensions
{
    public static class ConfigurationExtensions
    {
        public static ApacheKafkaConfig GetApacheKafkaConfigDefaults(this IConfiguration configuration)
            => configuration.GetSection(Constants.ConfigurationSections.ApacheKafka).Get<ApacheKafkaConfig>();

        public static void BindConsumerConfig(this IConfiguration configuration, string name, object config)
            => configuration.GetSection(Constants.ConfigurationSections.GetConsumerSection(name)).Bind(config);

        public static void BindProducerConfig(this IConfiguration configuration, string name, object config)
            => configuration.GetSection(Constants.ConfigurationSections.GetProducerSection(name)).Bind(config);
    }
}
