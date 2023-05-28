namespace NetTemplate.Blog.ApplicationCore.User.Events
{
    public class UserCreatedEvent
    {
        public UserPartialEntity Entity { get; }

        public UserCreatedEvent(UserPartialEntity entity)
        {
            Entity = entity;
        }
    }
}
