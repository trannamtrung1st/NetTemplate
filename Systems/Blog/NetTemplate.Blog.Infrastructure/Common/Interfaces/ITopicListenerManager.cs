namespace NetTemplate.Blog.Infrastructure.Common.Interfaces
{
    public interface ITopicListenerManager
    {
        Task StartListeners(CancellationToken cancellationToken = default);
    }
}
