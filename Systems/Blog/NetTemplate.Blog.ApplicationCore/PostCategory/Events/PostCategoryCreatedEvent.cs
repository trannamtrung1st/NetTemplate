namespace NetTemplate.Blog.ApplicationCore.PostCategory.Events
{
    public class PostCategoryCreatedEvent
    {
        public PostCategoryEntity Entity { get; }

        public PostCategoryCreatedEvent(PostCategoryEntity entity)
        {
            Entity = entity;
        }
    }
}
