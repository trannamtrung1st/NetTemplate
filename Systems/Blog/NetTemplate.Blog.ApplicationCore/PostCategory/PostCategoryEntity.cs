using NetTemplate.Blog.ApplicationCore.Common.Models;
using NetTemplate.Blog.ApplicationCore.Post;
using NetTemplate.Blog.ApplicationCore.PostCategory.Events;
using NetTemplate.Blog.ApplicationCore.User;
using NetTemplate.Common.Validation;
using NetTemplate.Shared.ApplicationCore.Common.Entities;
using NetTemplate.Shared.ApplicationCore.Common.Exceptions;
using NetTemplate.Shared.ApplicationCore.Common.Utils;
using CommonMessages = NetTemplate.Shared.ApplicationCore.Common.Constants.Messages;

namespace NetTemplate.Blog.ApplicationCore.PostCategory
{
    public class PostCategoryEntity : AppFullAuditableEntity<int>, IAggregateRoot, IHasCreator
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

        public PostCategoryEntity(int id)
        {
            Id = id;
        }

        public PostCategoryEntity(string name) : this()
        {
            object _ = ValidateNew(name, out Exception ex) ? null : throw ex;

            Name = name;

            QueuePipelineEvent(new PostCategoryCreatedEvent(this));
        }

        public void Update(string name)
        {
            object _ = ValidateUpdate(name, out Exception ex) ? null : throw ex;

            Name = name;

            QueuePipelineEvent(new PostCategoryUpdatedEvent(this.Id, name));
        }

        public override void SoftDelete()
        {
            object _ = ValidateDelete(out Exception ex) ? null : throw ex;

            base.SoftDelete();

            QueuePipelineEvent(new PostCategoryDeletedEvent(this.Id));
        }

        #region Validation rules

        private bool ValidateDelete(out Exception ex)
        {
            ex = null;

            return ex == null;
        }

        private bool ValidateUpdate(string name, out Exception ex)
        {
            List<string> invalidFields = new List<string>();

            invalidFields.AddRange(ValidateCommon(name));

            ex = invalidFields.Count > 0 ? new InvalidEntityDataException(invalidFields.ToArray()) : null;

            return ex == null;
        }

        private bool ValidateNew(string name, out Exception ex)
        {
            List<string> invalidFields = new List<string>();

            invalidFields.AddRange(ValidateCommon(name));

            ex = invalidFields.Count > 0 ? new InvalidEntityDataException(invalidFields.ToArray()) : null;

            return ex == null;
        }

        private List<string> ValidateCommon(string name)
        {
            List<string> invalidFields = new List<string>();

            if (!ApplicationValidation.ValidateMaxLength(new[] { name }))
            {
                invalidFields.Add(CommonMessages.InvalidMaxLength);
            }

            if (!CommonValidation.StringNotEmptyOrWhitespace(new[] { name }))
            {
                invalidFields.Add(CommonMessages.MissingRequiredFields);
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
