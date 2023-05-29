using MediatR;
using NetTemplate.Blog.ApplicationCore.Cross.Interfaces;
using NetTemplate.Blog.ApplicationCore.Post;
using NetTemplate.Blog.ApplicationCore.Post.Events;
using NetTemplate.Blog.ApplicationCore.PostCategory;
using NetTemplate.Blog.ApplicationCore.PostCategory.Events;
using NetTemplate.Shared.ApplicationCore.Common.Events;

namespace NetTemplate.Blog.ApplicationCore.Cross.Handlers
{
    // [TODO] retest events
    public class UpdateEntityVersionHandler :
        INotificationHandler<PostEntityEvent<PostCategoryCreatedEvent>>,
        INotificationHandler<PostEntityEvent<PostCategoryUpdatedEvent>>,
        INotificationHandler<PostEntityEvent<PostCategoryDeletedEvent>>,
        INotificationHandler<PostEntityEvent<PostCreatedEvent>>,
        INotificationHandler<PostEntityEvent<PostUpdatedEvent>>,
        INotificationHandler<PostEntityEvent<PostDeletedEvent>>
    {
        private readonly IEntityVersionManager _manager;

        public UpdateEntityVersionHandler(IEntityVersionManager manager)
        {
            _manager = manager;
        }

        public async Task Handle(PostEntityEvent<PostCategoryUpdatedEvent> notification, CancellationToken cancellationToken)
        {
            await UpdatePostCategory(notification.Data.EntityId);
        }

        public async Task Handle(PostEntityEvent<PostCategoryDeletedEvent> notification, CancellationToken cancellationToken)
        {
            await RemovePostCategory(notification.Data.EntityId);
        }

        public async Task Handle(PostEntityEvent<PostUpdatedEvent> notification, CancellationToken cancellationToken)
        {
            await UpdatePost(notification.Data.EntityId);
        }

        public async Task Handle(PostEntityEvent<PostDeletedEvent> notification, CancellationToken cancellationToken)
        {
            await RemovePost(notification.Data.EntityId);
        }

        public async Task Handle(PostEntityEvent<PostCategoryCreatedEvent> notification, CancellationToken cancellationToken)
        {
            await UpdatePostCategory(notification.Data.Entity.Id);
        }

        public async Task Handle(PostEntityEvent<PostCreatedEvent> notification, CancellationToken cancellationToken)
        {
            await UpdatePost(notification.Data.Entity.Id);
        }

        private async Task UpdatePostCategory(int id) => await _manager.UpdateVersion(nameof(PostCategoryEntity), id.ToString());
        private async Task RemovePostCategory(int id) => await _manager.Remove(nameof(PostCategoryEntity), id.ToString());
        private async Task UpdatePost(int id) => await _manager.UpdateVersion(nameof(PostEntity), id.ToString());
        private async Task RemovePost(int id) => await _manager.Remove(nameof(PostEntity), id.ToString());
    }
}
