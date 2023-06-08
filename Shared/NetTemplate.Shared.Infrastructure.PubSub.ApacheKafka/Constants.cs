using NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Models;

namespace NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka
{
    public static class Constants
    {
        public static class ConfigurationSections
        {
            public const string ApacheKafka = nameof(ApacheKafkaConfig);
            public const string Consumers = "ApacheKafkaConsumers";
            public const string Producers = "ApacheKafkaProducers";

            public static string GetConsumerSection(string name) => $"{Consumers}:{name}";
            public static string GetProducerSection(string name) => $"{Producers}:{name}";
        }
    }
}
