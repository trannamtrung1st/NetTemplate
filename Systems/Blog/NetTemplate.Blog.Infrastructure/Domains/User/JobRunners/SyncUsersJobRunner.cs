using Hangfire;
using NetTemplate.Blog.ApplicationCore.User.Jobs.SyncUsers;
using NetTemplate.Common.DependencyInjection.Attributes;
using NetTemplate.Shared.Infrastructure.Background.Filters;

namespace NetTemplate.Blog.Infrastructure.Domains.User.JobRunners
{
    public interface ISyncUsersJobRunner
    {
        [AutomaticRetry(Attempts = SyncUsersJobConstants.DefaultAttempts, DelaysInSeconds = new[]
        {
            SyncUsersJobConstants.InitialDelay,
            (int)(SyncUsersJobConstants.InitialDelay * 1.25),
            (int)(SyncUsersJobConstants.InitialDelay * 1.5)
        })]
        [IgnoreIfTimedOut(timeOutMs: SyncUsersJobConstants.TimeOutMs)]
        Task Start(SyncUsersJobArgument args, CancellationToken cancellationToken = default);
    }

    [ScopedService]
    public class SyncUsersJobRunner : ISyncUsersJobRunner
    {
        private readonly ISyncUsersJob _job;

        public SyncUsersJobRunner(ISyncUsersJob job)
        {
            _job = job;
        }

        public Task Start(SyncUsersJobArgument args, CancellationToken cancellationToken = default)
        {
            return _job.Start(args, cancellationToken);
        }
    }
}
