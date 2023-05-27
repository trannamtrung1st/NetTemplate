using MediatR;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Events
{
    public class PostCategoryUpdatedEvent : INotification
    {
        public int EntityId { get; }
        public string Name { get; }

        public PostCategoryUpdatedEvent(
            int entityId,
            string name)
        {
            EntityId = entityId;
            Name = name;
        }
    }
}
