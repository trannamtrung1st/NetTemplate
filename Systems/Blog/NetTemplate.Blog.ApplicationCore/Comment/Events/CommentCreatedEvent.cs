using MediatR;

namespace NetTemplate.Blog.ApplicationCore.Comment.Events
{
    public class CommentCreatedEvent : INotification
    {
        public CommentEntity Entity { get; }

        public CommentCreatedEvent(CommentEntity entity)
        {
            Entity = entity;
        }
    }
}
