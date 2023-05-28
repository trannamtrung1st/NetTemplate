using MediatR;

namespace NetTemplate.Shared.ApplicationCore.Common.Events
{
    public class ApplicationStartingEvent : INotification
    {
        public dynamic Data { get; }

        public ApplicationStartingEvent(dynamic data)
        {
            Data = data;
        }
    }
}
