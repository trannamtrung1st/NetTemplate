using MediatR;
using NetTemplate.Blog.ApplicationCore.PostCategory;
using NetTemplate.Shared.ApplicationCore.Common.Events;

namespace NetTemplate.Blog.Infrastructure.Domains.PostCategory.Handlers
{
    public class ApplicationStartingHandler : INotificationHandler<ApplicationStartingEvent>
    {
        private readonly IPostCategoryViewManager _postCategoryViewManager;

        public ApplicationStartingHandler(IPostCategoryViewManager postCategoryViewManager)
        {
            _postCategoryViewManager = postCategoryViewManager;
        }

        public async Task Handle(ApplicationStartingEvent @event, CancellationToken cancellationToken)
        {
            await _postCategoryViewManager.Initialize();
        }
    }
}
