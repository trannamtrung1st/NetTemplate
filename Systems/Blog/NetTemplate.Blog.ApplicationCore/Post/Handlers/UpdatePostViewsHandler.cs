using MediatR;
using NetTemplate.Blog.ApplicationCore.Post.Events;
using NetTemplate.Blog.ApplicationCore.Post.Interfaces;
using NetTemplate.Shared.ApplicationCore.Common.Events;

namespace NetTemplate.Blog.ApplicationCore.Post.Handlers
{
    public class UpdatePostViewsHandler :
        INotificationHandler<PostEntityEvent<PostDeletedEvent>>
    {
        private readonly IPostViewManager _postViewManager;

        public UpdatePostViewsHandler(IPostViewManager postViewManager)
        {
            _postViewManager = postViewManager;
        }

        public async Task Handle(PostEntityEvent<PostDeletedEvent> @event, CancellationToken cancellationToken)
        {
            await _postViewManager.UpdateViewsOnEvent(@event.Data);
        }
    }
}
