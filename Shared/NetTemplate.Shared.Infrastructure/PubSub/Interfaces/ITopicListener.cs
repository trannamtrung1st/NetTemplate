namespace NetTemplate.Shared.Infrastructure.PubSub.Interfaces
{
    public interface ITopicListener
    {
        bool Enabled { get; }
        Task Start(CancellationToken cancellationToken = default);
    }
}
