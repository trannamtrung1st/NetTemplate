using Microsoft.Extensions.Configuration;
using NetTemplate.ApacheKafka.Models;

namespace NetTemplate.ApacheKafka.Extensions
{
    public static class ConfigurationExtensions
    {
        public static ApacheKafkaConfig GetApacheKafkaConfigDefaults(this IConfiguration configuration)
            => configuration.GetSection(ApacheKafkaConfig.ConfigurationSection).Get<ApacheKafkaConfig>();

        public static IConfigurationSection GetConsumerConfig(this IConfiguration configuration, string name)
            => configuration.GetSection(ApacheKafkaConfig.GetConsumerSection(name));

        public static IConfigurationSection GetProducerConfig(this IConfiguration configuration, string name)
            => configuration.GetSection(ApacheKafkaConfig.GetProducerSection(name));
    }
}
