namespace NetTemplate.Blog.ApplicationCore.Post.Events
{
    public class PostTagsUpdatedEvent
    {
        public int EntityId { get; }
        public IEnumerable<string> Tags { get; }

        public PostTagsUpdatedEvent(int entityId, IEnumerable<string> tags)
        {
            EntityId = entityId;
            Tags = tags;
        }
    }
}
