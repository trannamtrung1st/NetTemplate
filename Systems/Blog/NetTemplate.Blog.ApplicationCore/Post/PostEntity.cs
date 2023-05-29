using NetTemplate.Blog.ApplicationCore.Post.Events;
using NetTemplate.Blog.ApplicationCore.PostCategory;
using NetTemplate.Blog.ApplicationCore.User;
using NetTemplate.Common.Validation;
using NetTemplate.Shared.ApplicationCore.Common.Entities;
using NetTemplate.Shared.ApplicationCore.Common.Exceptions;
using NetTemplate.Shared.ApplicationCore.Common.Utils;
using System.Linq.Expressions;
using CommonMessages = NetTemplate.Shared.ApplicationCore.Common.Constants.Messages;

namespace NetTemplate.Blog.ApplicationCore.Post
{
    public class PostEntity : AppFullAuditableEntity<int>, IAggregateRoot
    {
        public string Title { get; private set; }
        public string Content { get; private set; }
        public int CategoryId { get; private set; }

        #region Aggregate relationships

        private readonly List<PostTagEntity> _tags;
        public virtual IEnumerable<PostTagEntity> Tags => _tags;

        #endregion

        #region Non-aggregate relationships (Query-only)

        public virtual UserPartialEntity Creator { get; set; }
        public virtual PostCategoryEntity Category { get; set; }

        #endregion

        protected PostEntity() : base()
        {
            _tags = new List<PostTagEntity>();
        }

        public PostEntity(int id)
        {
            Id = id;
        }

        public PostEntity(
            string title, string content, int categoryId,
            IEnumerable<PostTagEntity> tags) : this()
        {
            object _ = ValidateNew(title, content, categoryId, tags, out Exception ex) ? null : throw ex;

            Title = title;
            Content = content;
            CategoryId = categoryId;

            if (tags?.Any() == true)
            {
                _tags.AddRange(tags);
            }

            QueuePipelineEvent(new PostCreatedEvent(this));
        }

        public void UpdatePost(string title, string content, int categoryId)
        {
            object _ = ValidateUpdate(title, content, categoryId, out Exception ex) ? null : throw ex;

            Title = title;
            Content = content;
            CategoryId = categoryId;

            QueuePipelineEvent(new PostUpdatedEvent(this.Id, title, content, categoryId));
        }

        public override void SoftDelete()
        {
            object _ = ValidateDelete(out Exception ex) ? null : throw ex;

            base.SoftDelete();

            QueuePipelineEvent(new PostDeletedEvent(this.Id));
        }

        public void UpdateTags(IEnumerable<string> updatedTags)
        {
            if (updatedTags == null) throw new ArgumentNullException(nameof(updatedTags));

            if (_tags == null) throw new InvalidEntityDataException(nameof(Tags));

            _tags.RemoveAll(currentTag => !updatedTags.Contains(currentTag.Value));

            PostTagEntity[] newTags = updatedTags
                .Where(t => !_tags.Any(currentTag => currentTag.Value == t))
                .Select(t => new PostTagEntity(t)).ToArray();

            _tags.AddRange(newTags);

            QueuePipelineEvent(new PostTagsUpdatedEvent(this.Id, updatedTags));
        }

        #region Validation rules

        private bool ValidateNew(
            string title, string content, int categoryId,
            IEnumerable<PostTagEntity> tags, out Exception ex)
        {
            List<string> invalidFields = new List<string>();

            invalidFields.AddRange(ValidatePostCommon(title, content, categoryId));

            ex = invalidFields.Count > 0 ? new InvalidEntityDataException(invalidFields.ToArray()) : null;

            return ex == null;
        }

        private bool ValidateUpdate(string title, string content, int categoryId, out Exception ex)
        {
            List<string> invalidFields = new List<string>();

            invalidFields.AddRange(ValidatePostCommon(title, content, categoryId));

            ex = invalidFields.Count > 0 ? new InvalidEntityDataException(invalidFields.ToArray()) : null;

            return ex == null;
        }

        private bool ValidateDelete(out Exception ex)
        {
            ex = null;

            return ex == null;
        }

        private List<string> ValidatePostCommon(string title, string content, int categoryId)
        {
            List<string> invalidFields = new List<string>();

            if (!ApplicationValidation.ValidateMaxLength(new[] { title }))
            {
                invalidFields.Add(CommonMessages.InvalidMaxLength);
            }

            if (!CommonValidation.StringNotEmptyOrWhitespace(new[] { title, content }))
            {
                invalidFields.Add(CommonMessages.MissingRequiredFields);
            }

            if (categoryId <= 0)
            {
                invalidFields.Add(nameof(CategoryId));
            }

            return invalidFields;
        }

        #endregion

        public override int TransientIdValue() => default;

        // [NOTE] exclude big data: Content
        public static Expression<Func<PostEntity, PostEntity>> SelectBasicInfoExpression
            => e => new PostEntity
            {
                Id = e.Id,
                CategoryId = e.CategoryId,
                CreatedTime = e.CreatedTime,
                CreatorId = e.CreatorId,
                DeletedTime = e.DeletedTime,
                DeletorId = e.DeletorId,
                IsDeleted = e.IsDeleted,
                LastModifiedTime = e.LastModifiedTime,
                LastModifyUserId = e.LastModifyUserId,
                Title = e.Title
            };

        public static Expression<Func<PostEntity, string>> CreatorFullNameExpression
            => (e) => e.Creator.Id > 0 ? e.Creator.FirstName + " " + e.Creator.LastName : null;

        public static class Constraints
        {
        }
    }
}
