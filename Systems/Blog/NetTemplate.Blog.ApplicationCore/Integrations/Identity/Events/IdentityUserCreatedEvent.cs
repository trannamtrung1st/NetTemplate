using MediatR;
using NetTemplate.Shared.ApplicationCore.Domains.Identity.Models;

namespace NetTemplate.Blog.ApplicationCore.Integrations.Identity
{
    public class IdentityUserCreatedEvent : INotification
    {
        public IdentityUserCreatedEventModel Model { get; }

        public IdentityUserCreatedEvent(IdentityUserCreatedEventModel model)
        {
            Model = model;
        }
    }
}
