using MediatR;

namespace NetTemplate.Shared.ApplicationCore.Common.Entities
{
    public abstract class DomainEntity
    {
        private Queue<INotification> _preEvents;
        private Queue<INotification> _postEvents;

        public IEnumerable<INotification> TakePreEvents() => TakeEvents(GetSet(ref _preEvents));
        public IEnumerable<INotification> TakePostvents() => TakeEvents(GetSet(ref _postEvents));
        public bool HasPreEvents() => GetSet(ref _preEvents).Count > 0;
        public bool HasPostEvents() => GetSet(ref _postEvents).Count > 0;

        protected void QueueEvent(INotification notification, bool isPost = false)
        {
            Queue<INotification> events = isPost ? GetSet(ref _postEvents) : GetSet(ref _preEvents);

            events.Enqueue(notification);
        }

        protected void QueuePipelineEvent(INotification notification)
        {
            QueueEvent(notification, isPost: false);
            QueueEvent(notification, isPost: true);
        }

        private IEnumerable<INotification> TakeEvents(Queue<INotification> events)
        {
            while (events.Count > 0)
            {
                yield return events.Dequeue();
            }
        }

        private Queue<INotification> GetSet(ref Queue<INotification> events)
        {
            events ??= new Queue<INotification>();

            return events;
        }
    }
}
