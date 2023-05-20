using MediatR;

namespace NetTemplate.Blog.ApplicationCore.Post.Events
{
    public class PostDeletedEvent : INotification
    {
        public int EntityId { get; }

        public PostDeletedEvent(int entityId)
        {
            EntityId = entityId;
        }
    }
}
