using MediatR;
using NetTemplate.Blog.ApplicationCore.Post;
using NetTemplate.Blog.ApplicationCore.Post.Events;
using NetTemplate.Blog.ApplicationCore.PostCategory;
using NetTemplate.Blog.ApplicationCore.PostCategory.Events;
using NetTemplate.Shared.ApplicationCore.Common.Events;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;

namespace NetTemplate.Blog.ApplicationCore.Cross.Handlers
{
    public class UpdateEntityVersionHandler :
        INotificationHandler<PostEntityEvent<PostCategoryCreatedEvent>>,
        INotificationHandler<PostEntityEvent<PostCategoryUpdatedEvent>>,
        INotificationHandler<PostEntityEvent<PostCategoryDeletedEvent>>,
        INotificationHandler<PostEntityEvent<PostCreatedEvent>>,
        INotificationHandler<PostEntityEvent<PostUpdatedEvent>>,
        INotificationHandler<PostEntityEvent<PostTagsUpdatedEvent>>,
        INotificationHandler<PostEntityEvent<PostDeletedEvent>>
    {
        private readonly IEntityVersionManager _manager;

        public UpdateEntityVersionHandler(IEntityVersionManager manager)
        {
            _manager = manager;
        }

        public async Task Handle(PostEntityEvent<PostCategoryUpdatedEvent> notification, CancellationToken cancellationToken)
        {
            await UpdatePostCategory(notification.Data.EntityId, cancellationToken);
        }

        public async Task Handle(PostEntityEvent<PostCategoryDeletedEvent> notification, CancellationToken cancellationToken)
        {
            await RemovePostCategory(notification.Data.EntityId, cancellationToken);
        }

        public async Task Handle(PostEntityEvent<PostUpdatedEvent> notification, CancellationToken cancellationToken)
        {
            await UpdatePost(notification.Data.EntityId, cancellationToken);
        }

        public async Task Handle(PostEntityEvent<PostTagsUpdatedEvent> notification, CancellationToken cancellationToken)
        {
            await UpdatePost(notification.Data.EntityId, cancellationToken);
        }

        public async Task Handle(PostEntityEvent<PostDeletedEvent> notification, CancellationToken cancellationToken)
        {
            await RemovePost(notification.Data.EntityId, cancellationToken);
        }

        public async Task Handle(PostEntityEvent<PostCategoryCreatedEvent> notification, CancellationToken cancellationToken)
        {
            await UpdatePostCategory(notification.Data.Entity.Id, cancellationToken);
        }

        public async Task Handle(PostEntityEvent<PostCreatedEvent> notification, CancellationToken cancellationToken)
        {
            await UpdatePost(notification.Data.Entity.Id, cancellationToken);
        }

        private async Task UpdatePostCategory(int id, CancellationToken cancellationToken = default) => await _manager.UpdateVersion(nameof(PostCategoryEntity), id.ToString(), cancellationToken);
        private async Task RemovePostCategory(int id, CancellationToken cancellationToken = default) => await _manager.Remove(nameof(PostCategoryEntity), id.ToString(), cancellationToken);
        private async Task UpdatePost(int id, CancellationToken cancellationToken = default) => await _manager.UpdateVersion(nameof(PostEntity), id.ToString(), cancellationToken);
        private async Task RemovePost(int id, CancellationToken cancellationToken = default) => await _manager.Remove(nameof(PostEntity), id.ToString(), cancellationToken);
    }
}
