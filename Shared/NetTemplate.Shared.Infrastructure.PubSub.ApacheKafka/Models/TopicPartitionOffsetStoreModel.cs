using Confluent.Kafka;

namespace NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Models
{
    public class TopicPartitionOffsetStoreModel
    {
        public string Topic { get; set; }
        public int Partition { get; set; }
        public long Offset { get; set; }
        public int? LeaderEpoch { get; set; }

        public TopicPartitionOffsetStoreModel()
        {
        }

        public TopicPartitionOffsetStoreModel(TopicPartitionOffset currentOffset)
        {
            Topic = currentOffset.Topic;
            Partition = currentOffset.Partition;
            Offset = currentOffset.Offset + 1;
            LeaderEpoch = currentOffset.LeaderEpoch;
        }

        public TopicPartitionOffset ToOffset() => new TopicPartitionOffset(Topic, Partition, Offset, LeaderEpoch);
    }
}
