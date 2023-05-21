using NetTemplate.Blog.ApplicationCore.Comment.Events;
using NetTemplate.Blog.ApplicationCore.Post;
using NetTemplate.Blog.ApplicationCore.User;
using NetTemplate.Shared.ApplicationCore.Common.Entities;
using NetTemplate.Shared.ApplicationCore.Common.Exceptions;

namespace NetTemplate.Blog.ApplicationCore.Comment
{
    public class CommentEntity : AppFullAuditableEntity<int>, IAggregateRoot
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

        public CommentEntity(string content, int onPostId) : this()
        {
            object _ = ValidateNew(content, onPostId, out Exception ex) ? null : throw ex;

            Content = content;
            OnPostId = onPostId;

            QueuePipelineEvent(new CommentCreatedEvent(this));
        }

        #region Validation rules

        private bool ValidateNew(string content, int onPostId, out Exception ex)
        {
            List<string> invalidFields = new List<string>();

            if (string.IsNullOrWhiteSpace(content)
                || content.Length > Constraints.MaxContentLength)
            {
                invalidFields.Add(nameof(Content));
            }

            if (onPostId <= 0)
            {
                invalidFields.Add(nameof(OnPostId));
            }

            ex = invalidFields.Count > 0 ? new InvalidEntityDataException(invalidFields.ToArray()) : null;

            return ex == null;
        }

        #endregion

        public override int TransientIdValue() => default;

        public static class Constraints
        {
            public const int MaxContentLength = 500;
        }
    }
}
