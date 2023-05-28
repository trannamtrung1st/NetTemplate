namespace NetTemplate.Blog.ApplicationCore.Comment.Events
{
    public class CommentCreatedEvent
    {
        public CommentEntity Entity { get; }

        public CommentCreatedEvent(CommentEntity entity)
        {
            Entity = entity;
        }
    }
}
