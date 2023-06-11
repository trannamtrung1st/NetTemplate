namespace NetTemplate.Shared.Infrastructure.PubSub.Interfaces
{
    public interface ITopicListenerManager
    {
        Task StartListeners(CancellationToken cancellationToken = default);
    }
}
