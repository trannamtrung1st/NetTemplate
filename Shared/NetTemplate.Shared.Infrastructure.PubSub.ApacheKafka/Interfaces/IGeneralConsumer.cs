using NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Models;

namespace NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Interfaces
{
    public interface IGeneralConsumer
    {
        Task Start(CompetingConsumerConfig commonConfig, CancellationToken cancellationToken = default);
    }
}
