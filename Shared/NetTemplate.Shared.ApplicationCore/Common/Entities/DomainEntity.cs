using NetTemplate.Shared.ApplicationCore.Common.Events;

namespace NetTemplate.Shared.ApplicationCore.Common.Entities
{
    public abstract class DomainEntity
    {
        private Queue<EntityEvent> _preEvents;
        private Queue<EntityEvent> _postEvents;

        public IEnumerable<EntityEvent> TakePreEvents() => TakeEvents(GetSet(ref _preEvents));
        public IEnumerable<EntityEvent> TakePostvents() => TakeEvents(GetSet(ref _postEvents));
        public bool HasPreEvents() => GetSet(ref _preEvents).Count > 0;
        public bool HasPostEvents() => GetSet(ref _postEvents).Count > 0;

        protected void QueueEvent(EntityEvent notification, bool isPost = false)
        {
            Queue<EntityEvent> events = isPost ? GetSet(ref _postEvents) : GetSet(ref _preEvents);

            events.Enqueue(notification);
        }

        protected void QueuePreEvent<TEventData>(TEventData eventData)
        {
            QueueEvent(new PreEntityEvent<TEventData>(eventData), isPost: false);
        }

        protected void QueuePostEvent<TEventData>(TEventData eventData)
        {
            QueueEvent(new PostEntityEvent<TEventData>(eventData), isPost: true);
        }

        protected void QueuePipelineEvent<TEventData>(TEventData data)
        {
            QueuePreEvent(data);
            QueuePostEvent(data);
        }

        private IEnumerable<EntityEvent> TakeEvents(Queue<EntityEvent> events)
        {
            while (events.Count > 0)
            {
                yield return events.Dequeue();
            }
        }

        private Queue<EntityEvent> GetSet(ref Queue<EntityEvent> events)
        {
            events ??= new Queue<EntityEvent>();

            return events;
        }
    }
}
