using MediatR;
using NetTemplate.Blog.ApplicationCore.PostCategory.Events;
using NetTemplate.Blog.ApplicationCore.PostCategory.Interfaces;
using NetTemplate.Shared.ApplicationCore.Common.Events;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Handlers
{
    public class UpdatePostCategoryViewsHandler :
        INotificationHandler<PostEntityEvent<PostCategoryCreatedEvent>>,
        INotificationHandler<PostEntityEvent<PostCategoryUpdatedEvent>>,
        INotificationHandler<PostEntityEvent<PostCategoryDeletedEvent>>
    {
        private readonly IPostCategoryViewManager _postCategoryViewManager;

        public UpdatePostCategoryViewsHandler(IPostCategoryViewManager postCategoryViewManager)
        {
            _postCategoryViewManager = postCategoryViewManager;
        }

        public async Task Handle(PostEntityEvent<PostCategoryCreatedEvent> @event, CancellationToken cancellationToken)
        {
            await _postCategoryViewManager.UpdateViewsOnEvent(@event.Data);
        }

        public async Task Handle(PostEntityEvent<PostCategoryDeletedEvent> @event, CancellationToken cancellationToken)
        {
            await _postCategoryViewManager.UpdateViewsOnEvent(@event.Data);
        }

        public async Task Handle(PostEntityEvent<PostCategoryUpdatedEvent> @event, CancellationToken cancellationToken)
        {
            await _postCategoryViewManager.UpdateViewsOnEvent(@event.Data);
        }
    }
}
