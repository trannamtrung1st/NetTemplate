namespace NetTemplate.Shared.Infrastructure.PubSub.Interfaces
{
    public interface ITopicManager
    {
        Task UpdateTopics(CancellationToken cancellationToken = default);
    }
}
