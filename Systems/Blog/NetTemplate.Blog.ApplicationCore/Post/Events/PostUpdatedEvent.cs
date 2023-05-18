using MediatR;

namespace NetTemplate.Blog.ApplicationCore.Post.Events
{
    public class PostUpdatedEvent : INotification
    {
        public int EntityId { get; }
        public string Title { get; }
        public string Content { get; }
        public int CategoryId { get; }

        public PostUpdatedEvent(
            int entityId,
            string title,
            string content,
            int categoryId)
        {
            EntityId = entityId;
            Title = title;
            Content = content;
            CategoryId = categoryId;
        }
    }
}
