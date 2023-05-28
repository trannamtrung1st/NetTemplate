namespace NetTemplate.Blog.ApplicationCore.Post.Events
{
    public class PostCreatedEvent
    {
        public PostEntity Entity { get; }

        public PostCreatedEvent(PostEntity entity)
        {
            Entity = entity;
        }
    }
}
