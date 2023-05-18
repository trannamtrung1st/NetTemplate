using MediatR;

namespace NetTemplate.Blog.ApplicationCore.Post.Events
{
    public class PostCreatedEvent : INotification
    {
        public PostEntity Entity { get; }

        public PostCreatedEvent(PostEntity entity)
        {
            Entity = entity;
        }
    }
}
