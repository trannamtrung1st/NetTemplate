using MediatR;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Events
{
    public class PostCategoryCreatedEvent : INotification
    {
        public PostCategoryEntity Entity { get; }

        public PostCategoryCreatedEvent(PostCategoryEntity entity)
        {
            Entity = entity;
        }
    }
}
