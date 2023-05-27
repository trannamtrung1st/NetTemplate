using MediatR;
using NetTemplate.Blog.ApplicationCore.Post;
using NetTemplate.Shared.ApplicationCore.Common.Events;

namespace NetTemplate.Blog.Infrastructure.Domains.Post.Handlers
{
    public class ApplicationStartingEventHandler : INotificationHandler<ApplicationStartingEvent>
    {
        private readonly IPostViewManager _postViewManager;

        public ApplicationStartingEventHandler(IPostViewManager postViewManager)
        {
            _postViewManager = postViewManager;
        }

        public async Task Handle(ApplicationStartingEvent @event, CancellationToken cancellationToken)
        {
            await _postViewManager.Initialize();
        }
    }
}
