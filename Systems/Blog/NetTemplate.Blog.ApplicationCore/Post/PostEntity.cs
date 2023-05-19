using NetTemplate.Blog.ApplicationCore.Post.Events;
using NetTemplate.Blog.ApplicationCore.PostCategory;
using NetTemplate.Blog.ApplicationCore.User;
using NetTemplate.Common.Validation;
using NetTemplate.Shared.ApplicationCore.Constants;
using NetTemplate.Shared.ApplicationCore.Entities;
using NetTemplate.Shared.ApplicationCore.Exceptions;
using NetTemplate.Shared.ApplicationCore.Utils;

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

        public void UpdateTags(IEnumerable<PostTagEntity> tags)
        {
            if (tags == null) throw new ArgumentNullException(nameof(tags));

            if (_tags == null) throw new InvalidEntityDataException(nameof(Tags));

            _tags.RemoveAll(currentTag => !tags.Contains(currentTag));

            PostTagEntity[] newTags = tags.Where(t => t.IsTransient()).ToArray();

            _tags.AddRange(newTags);

            // [NOTE] fire event if necessary
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

        private List<string> ValidatePostCommon(string title, string content, int categoryId)
        {
            List<string> invalidFields = new List<string>();

            if (!ApplicationValidation.ValidateMaxLength(new[] { title }))
            {
                invalidFields.Add(Messages.Common.InvalidMaxLength);
            }

            if (!CommonValidation.StringNotEmptyOrWhitespace(new[] { title, content }))
            {
                invalidFields.Add(Messages.Common.MissingRequiredFields);
            }

            if (categoryId <= 0)
            {
                invalidFields.Add(nameof(CategoryId));
            }

            return invalidFields;
        }

        #endregion

        public override int TransientIdValue() => default;

        public static class Constraints
        {
        }
    }
}
