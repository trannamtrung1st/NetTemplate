namespace NetTemplate.Blog.ApplicationCore.Post.Events
{
    public class PostDeletedEvent
    {
        public int EntityId { get; }

        public PostDeletedEvent(int entityId)
        {
            EntityId = entityId;
        }
    }
}
