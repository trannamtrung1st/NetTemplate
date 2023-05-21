using NetTemplate.Blog.ApplicationCore.Post;
using NetTemplate.Blog.ApplicationCore.PostCategory.Events;
using NetTemplate.Blog.ApplicationCore.User;
using NetTemplate.Shared.ApplicationCore.Common.Constants;
using NetTemplate.Shared.ApplicationCore.Common.Entities;
using NetTemplate.Shared.ApplicationCore.Common.Exceptions;
using NetTemplate.Shared.ApplicationCore.Common.Utils;

namespace NetTemplate.Blog.ApplicationCore.PostCategory
{
    public class PostCategoryEntity : AppFullAuditableEntity<int>, IAggregateRoot
    {
        public string Name { get; private set; }

        #region Aggregate relationships

        #endregion

        #region Non-aggregate relationships (Query-only)

        public virtual UserPartialEntity Creator { get; set; }
        public virtual IEnumerable<PostEntity> Posts { get; set; }

        #endregion

        protected PostCategoryEntity() : base()
        {
        }

        public PostCategoryEntity(string name) : this()
        {
            object _ = ValidateNew(name, out Exception ex) ? null : throw ex;

            Name = name;

            QueuePipelineEvent(new PostCategoryCreatedEvent(this));
        }

        #region Validation rules

        private bool ValidateNew(string name, out Exception ex)
        {
            List<string> invalidFields = new List<string>();

            if (!ApplicationValidation.ValidateMaxLength(new[] { name }))
            {
                invalidFields.Add(Messages.Common.InvalidMaxLength);
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                invalidFields.Add(nameof(Name));
            }

            ex = invalidFields.Count > 0 ? new InvalidEntityDataException(invalidFields.ToArray()) : null;

            return ex == null;
        }

        #endregion

        public override int TransientIdValue() => default;

        public static class Constraints
        {
        }
    }
}
