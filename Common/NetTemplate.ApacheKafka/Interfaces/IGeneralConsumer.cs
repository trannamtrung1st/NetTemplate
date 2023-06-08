namespace NetTemplate.ApacheKafka.Interfaces
{
    public interface IGeneralConsumer
    {
        bool Enabled { get; }
        Task Start(CancellationToken cancellationToken = default);
    }
}
