using MediatR;

namespace NetTemplate.Blog.ApplicationCore.User.Events
{
    public class UserCreatedEvent : INotification
    {
        public UserPartialEntity Entity { get; }

        public UserCreatedEvent(UserPartialEntity entity)
        {
            Entity = entity;
        }
    }
}
