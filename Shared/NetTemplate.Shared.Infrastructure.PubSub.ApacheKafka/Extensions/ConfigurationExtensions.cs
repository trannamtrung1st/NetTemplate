using Microsoft.Extensions.Configuration;
using NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Models;

namespace NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Extensions
{
    public static class ConfigurationExtensions
    {
        public static ApacheKafkaConfig GetApacheKafkaConfigDefaults(this IConfiguration configuration)
            => configuration.GetSection(Constants.ConfigurationSections.ApacheKafka).Get<ApacheKafkaConfig>();

        public static IConfigurationSection GetConsumerConfig(this IConfiguration configuration, string name)
            => configuration.GetSection(Constants.ConfigurationSections.GetConsumerSection(name));

        public static IConfigurationSection GetProducerConfig(this IConfiguration configuration, string name)
            => configuration.GetSection(Constants.ConfigurationSections.GetProducerSection(name));
    }
}
