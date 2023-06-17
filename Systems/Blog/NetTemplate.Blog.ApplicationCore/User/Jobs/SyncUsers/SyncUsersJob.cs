using MediatR;
using Microsoft.Extensions.Logging;
using NetTemplate.Blog.ApplicationCore.User.Commands.SyncUsers;
using NetTemplate.Common.DependencyInjection.Attributes;
using NetTemplate.Common.Synchronization.Interfaces;

namespace NetTemplate.Blog.ApplicationCore.User.Jobs.SyncUsers
{
    public interface ISyncUsersJob
    {
        Task Start(SyncUsersJobArgument args, CancellationToken cancellationToken = default);
    }

    [ScopedService]
    public class SyncUsersJob : ISyncUsersJob
    {
        static readonly TimeSpan DefaultLockExpiry = TimeSpan.FromMinutes(7);

        private readonly IMediator _mediator;
        private readonly IDistributedLock _distributedLock;
        private readonly ILogger<SyncUsersJob> _logger;

        public SyncUsersJob(IMediator mediator,
            IDistributedLock distributedLock,
            ILogger<SyncUsersJob> logger)
        {
            _mediator = mediator;
            _distributedLock = distributedLock;
            _logger = logger;
        }

        public async Task Start(SyncUsersJobArgument args, CancellationToken cancellationToken = default)
        {
            // [NOTE] can add validator if necessary

            using ILockObject lockObj = await _distributedLock.CreateLock(
                resource: nameof(SyncUsersJob),
                expiryTime: DefaultLockExpiry,
                waitTime: TimeSpan.Zero,
                retryTime: TimeSpan.Zero,
                cancellationToken);

            if (lockObj.IsAcquired)
            {
                await _mediator.Send(new SyncUsersCommand(), cancellationToken);
            }
            else
            {
                _logger.LogWarning($"{nameof(SyncUsersJob)} is already running!");
            }
        }
    }
}
