using NetTemplate.Blog.ApplicationCore.Comment.Events;
using NetTemplate.Blog.ApplicationCore.Common.Models;
using NetTemplate.Blog.ApplicationCore.Post;
using NetTemplate.Blog.ApplicationCore.User;
using NetTemplate.Shared.ApplicationCore.Common.Entities;
using NetTemplate.Shared.ApplicationCore.Common.Exceptions;

namespace NetTemplate.Blog.ApplicationCore.Comment
{
    public class CommentEntity : AppFullAuditableEntity<int>, IAggregateRoot, IHasCreator
    {
        public string Content { get; private set; }
        public int OnPostId { get; private set; }

        #region Aggregate relationships

        #endregion

        #region Non-aggregate relationships (Query-only)

        public virtual UserPartialEntity Creator { get; set; }
        public virtual PostEntity OnPost { get; set; }

        #endregion

        protected CommentEntity() : base()
        {
        }

        public CommentEntity(int id)
        {
            Id = id;
        }

        public CommentEntity(string content, int onPostId) : this()
        {
            object _ = ValidateNew(content, onPostId, out Exception ex) ? null : throw ex;

            Content = content;
            OnPostId = onPostId;

            QueuePipelineEvent(new CommentCreatedEvent(this));
        }


        public void Update(string content)
        {
            object _ = ValidateUpdate(content, out Exception ex) ? null : throw ex;

            Content = content;

            // [NOTE] fire event if necessary
        }

        public override void SoftDelete()
        {
            object _ = ValidateDelete(out Exception ex) ? null : throw ex;

            base.SoftDelete();

            // [NOTE] fire event if necessary
        }

        #region Validation rules

        private bool ValidateDelete(out Exception ex)
        {
            ex = null;

            return ex == null;
        }

        private bool ValidateUpdate(string content, out Exception ex)
        {
            List<string> invalidFields = new List<string>();

            invalidFields.AddRange(ValidateCommon(content));

            ex = invalidFields.Count > 0 ? new InvalidEntityDataException(invalidFields.ToArray()) : null;

            return ex == null;
        }

        private bool ValidateNew(string content, int onPostId, out Exception ex)
        {
            List<string> invalidFields = new List<string>();

            invalidFields.AddRange(ValidateCommon(content));

            if (onPostId <= 0)
            {
                invalidFields.Add(nameof(OnPostId));
            }

            ex = invalidFields.Count > 0 ? new InvalidEntityDataException(invalidFields.ToArray()) : null;

            return ex == null;
        }

        private List<string> ValidateCommon(string content)
        {
            List<string> invalidFields = new List<string>();

            if (string.IsNullOrWhiteSpace(content)
                || content.Length > Constraints.MaxContentLength)
            {
                invalidFields.Add(nameof(Content));
            }

            return invalidFields;
        }

        #endregion

        public static class Constraints
        {
            public const int MaxContentLength = 500;
        }
    }
}
