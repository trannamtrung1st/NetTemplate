using MediatR;
using NetTemplate.Blog.ApplicationCore.PostCategory;
using NetTemplate.Shared.ApplicationCore.Common.Events;

namespace NetTemplate.Blog.Infrastructure.Domains.PostCategory.Handlers
{
    public class ApplicationStartingEventHandler : INotificationHandler<ApplicationStartingEvent>
    {
        private readonly IPostCategoryViewManager _postCategoryViewManager;

        public ApplicationStartingEventHandler(IPostCategoryViewManager postCategoryViewManager)
        {
            _postCategoryViewManager = postCategoryViewManager;
        }

        public async Task Handle(ApplicationStartingEvent @event, CancellationToken cancellationToken)
        {
            await _postCategoryViewManager.Initialize();
        }
    }
}
