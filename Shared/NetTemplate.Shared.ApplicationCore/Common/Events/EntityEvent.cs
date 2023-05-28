using MediatR;

namespace NetTemplate.Shared.ApplicationCore.Common.Events
{
    public abstract class EntityEvent : INotification
    {
    }

    public abstract class EntityEvent<TEventData> : EntityEvent
    {
        public TEventData Data { get; }

        public EntityEvent(TEventData data)
        {
            Data = data;
        }
    }

    public class PreEntityEvent<TEventData> : EntityEvent<TEventData>
    {
        public PreEntityEvent(TEventData data) : base(data)
        {
        }
    }

    public class PostEntityEvent<TEventData> : EntityEvent<TEventData>
    {
        public PostEntityEvent(TEventData data) : base(data)
        {
        }
    }
}
