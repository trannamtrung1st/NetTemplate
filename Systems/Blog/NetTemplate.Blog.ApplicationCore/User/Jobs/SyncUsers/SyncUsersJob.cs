using MediatR;
using NetTemplate.Blog.ApplicationCore.User.Commands.SyncUsers;
using NetTemplate.Common.DependencyInjection;

namespace NetTemplate.Blog.ApplicationCore.User.Jobs.SyncUsers
{
    public interface ISyncUsersJob
    {
        Task Start(SyncUsersJobArgument args);
    }

    [ScopedService]
    public class SyncUsersJob : ISyncUsersJob
    {
        private readonly IMediator _mediator;

        public SyncUsersJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Start(SyncUsersJobArgument args)
        {
            // [NOTE] can add validator if necessary

            await _mediator.Send(new SyncUsersCommand());
        }
    }
}
