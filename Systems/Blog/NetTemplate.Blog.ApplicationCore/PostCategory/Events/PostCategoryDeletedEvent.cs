using MediatR;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Events
{
    public class PostCategoryDeletedEvent : INotification
    {
        public int EntityId { get; }

        public PostCategoryDeletedEvent(int entityId)
        {
            EntityId = entityId;
        }
    }
}
