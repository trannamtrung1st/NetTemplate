using MediatR;

namespace NetTemplate.Shared.ApplicationCore.Entities
{
    public abstract class DomainEntity
    {
        private Queue<INotification> _preEvents;
        private Queue<INotification> _postEvents;

        public IEnumerable<INotification> TakePreEvents() => TakeEvents(_preEvents);
        public IEnumerable<INotification> TakePostvents() => TakeEvents(_postEvents);

        protected void QueueEvent(INotification notification, bool isPost = false)
        {
            Queue<INotification> events = isPost ? _postEvents : _preEvents;

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
    }
}
