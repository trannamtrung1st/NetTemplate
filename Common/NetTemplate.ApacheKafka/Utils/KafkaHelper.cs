using Confluent.Kafka;
using NetTemplate.ApacheKafka.Serdes;

namespace NetTemplate.ApacheKafka.Utils
{
    public static class KafkaHelper
    {
        public static IProducer<TKey, TValue> CreateProducer<TKey, TValue>(ProducerConfig config)
        {
            ProducerBuilder<TKey, TValue> builder = new ProducerBuilder<TKey, TValue>(config);

            Type valueType = typeof(TValue);

            if (valueType.IsClass || valueType.IsInterface)
            {
                builder.SetValueSerializer(new SimpleJsonSerdes<TValue>());
            }

            IProducer<TKey, TValue> producer = builder.Build();

            return producer;
        }


        public static IConsumer<TKey, TValue> CreateConsumer<TKey, TValue>(ConsumerConfig config)
        {
            ConsumerBuilder<TKey, TValue> builder = new ConsumerBuilder<TKey, TValue>(config);

            Type valueType = typeof(TValue);

            if (valueType.IsClass || valueType.IsInterface)
            {
                builder.SetValueDeserializer(new SimpleJsonSerdes<TValue>());
            }

            IConsumer<TKey, TValue> consumer = builder.Build();

            return consumer;
        }

        public static IAdminClient CreateAdmin(AdminClientConfig config)
        {
            return new AdminClientBuilder(config).Build();
        }
    }
}
