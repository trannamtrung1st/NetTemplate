namespace NetTemplate.Blog.Infrastructure.Common.Interfaces
{
    public interface ITopicManager
    {
        Task UpdateTopics(CancellationToken cancellationToken = default);
    }
}
