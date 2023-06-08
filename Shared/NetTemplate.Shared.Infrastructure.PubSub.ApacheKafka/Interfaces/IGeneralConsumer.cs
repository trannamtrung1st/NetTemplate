namespace NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Interfaces
{
    public interface IGeneralConsumer
    {
        bool Enabled { get; }
        Task Start(CancellationToken cancellationToken = default);
    }
}
