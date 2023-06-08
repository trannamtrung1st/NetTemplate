using Confluent.Kafka;

namespace NetTemplate.ApacheKafka.Interfaces
{
    public interface IExternalOffsetStore
    {
        Task<IEnumerable<TopicPartitionOffset>> GetStoredOffsets(IEnumerable<string> topics, string groupId, CancellationToken cancellationToken = default);
        Task StoreOffsets(IEnumerable<TopicPartitionOffset> offsets, string groupId, CancellationToken cancellationToken = default);
    }
}
