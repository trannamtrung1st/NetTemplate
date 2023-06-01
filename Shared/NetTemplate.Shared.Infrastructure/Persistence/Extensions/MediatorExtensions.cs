using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NetTemplate.Shared.ApplicationCore.Common.Entities;

namespace NetTemplate.Shared.Infrastructure.Persistence.Extensions
{
    static class MediatorExtension
    {
        public static async Task DispatchEntityEventsAsync(this IMediator mediator,
            DbContext ctx, bool isPost, CancellationToken cancellationToken = default)
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
                    cancellationToken.ThrowIfCancellationRequested();

                    IEnumerable<INotification> events = isPost ? entry.Entity.TakePostvents() : entry.Entity.TakePreEvents();

                    foreach (INotification @event in events)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        await mediator.Publish(@event, cancellationToken);
                    }
                }

            } while (entities.Length > 0);
        }
    }
}
