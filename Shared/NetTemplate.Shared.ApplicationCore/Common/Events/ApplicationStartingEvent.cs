using MediatR;

namespace NetTemplate.Shared.ApplicationCore.Common.Events
{
    public class ApplicationStartingEvent : INotification
    {
        public dynamic Data { get; set; }
    }
}
