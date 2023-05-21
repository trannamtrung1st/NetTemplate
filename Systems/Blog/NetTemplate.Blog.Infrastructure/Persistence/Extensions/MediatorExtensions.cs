using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NetTemplate.Shared.ApplicationCore.Entities;

namespace NetTemplate.Blog.Infrastructure.Persistence.Extensions
{
    static class MediatorExtension
    {
        public static async Task DispatchEntityEventsAsync(this IMediator mediator,
            MainDbContext ctx, bool isPost)
        {
            EntityEntry<DomainEntity>[] entities;

            do
            {
                entities = ctx.ChangeTracker
                    .Entries<DomainEntity>()
                    .Where(x => isPost ? x.Entity.HasPostEvents() : x.Entity.HasPreEvents())
                    .ToArray();

                foreach (EntityEntry<DomainEntity> entry in entities)
                {
                    IEnumerable<INotification> events = isPost ? entry.Entity.TakePostvents() : entry.Entity.TakePreEvents();

                    foreach (INotification @event in events)
                    {
                        await mediator.Publish(@event);
                    }
                }

            } while (entities.Length > 0);
        }
    }
}
