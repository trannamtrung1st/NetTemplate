using Confluent.Kafka;
using NetTemplate.ApacheKafka.Interfaces;
using NetTemplate.ApacheKafka.Models;
using NetTemplate.Common.MemoryStore.Interfaces;

namespace NetTemplate.ApacheKafka.Implementations
{
    public class MemoryOffsetStore : IExternalOffsetStore
    {
        private readonly IMemoryStore _memoryStore;
        private readonly IAdminClient _adminClient;

        public MemoryOffsetStore(IMemoryStore memoryStore, IAdminClient adminClient)
        {
            _memoryStore = memoryStore;
            _adminClient = adminClient;
        }

        public async Task<IEnumerable<TopicPartitionOffset>> GetStoredOffsets(IEnumerable<string> topics, string groupId, CancellationToken cancellationToken = default)
        {
            List<string> keys = new List<string>();
            List<TopicPartitionOffset> offsets = new List<TopicPartitionOffset>();

            foreach (string topic in topics)
            {
                Metadata metadata = _adminClient.GetMetadata(topic, Constants.DefaultTimeout);

                TopicMetadata topicMetadata = metadata.Topics.FirstOrDefault();

                if (topicMetadata != null)
                {
                    IEnumerable<string> topicKeys = topicMetadata.Partitions.Select(p => GetKey(topic, p.PartitionId, groupId));

                    keys.AddRange(topicKeys);
                }
            }

            foreach (string key in keys)
            {
                TopicPartitionOffsetStoreModel offset = await _memoryStore.HashGet<TopicPartitionOffsetStoreModel>(Constants.ContainerKey, key, cancellationToken);

                if (offset != null) offsets.Add(offset.ToOffset());
            }

            return offsets;
        }

        public async Task StoreOffsets(IEnumerable<TopicPartitionOffset> offsets, string groupId, CancellationToken cancellationToken = default)
        {
            foreach (TopicPartitionOffset offset in offsets)
            {
                string key = GetKey(offset.Topic, offset.Partition, groupId);

                TopicPartitionOffsetStoreModel currentOffset = await _memoryStore.HashGet<TopicPartitionOffsetStoreModel>(Constants.ContainerKey, key, cancellationToken);

                if (currentOffset == null || currentOffset.Offset < offset.Offset + 1)
                {
                    await _memoryStore.HashSet(Constants.ContainerKey, key, new TopicPartitionOffsetStoreModel(offset), cancellationToken);
                }
            }
        }

        private static string GetKey(string topic, int partition, string groupId)
            => $"{Constants.KeyPrefix}_{groupId}_{topic}_{partition}";

        private static class Constants
        {
            public const string ContainerKey = nameof(MemoryOffsetStore);
            public const string KeyPrefix = nameof(MemoryOffsetStore);
            public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(10);
        }
    }
}
