using NetTemplate.Blog.ApplicationCore.User.Events;
using NetTemplate.Shared.ApplicationCore.Entities;
using NetTemplate.Shared.ApplicationCore.Exceptions;

namespace NetTemplate.Blog.ApplicationCore.User
{
    public class UserPartialEntity : AppFullAuditableEntity<int>, IAggregateRoot
    {
        public string EmpCode { get; private set; }

        protected UserPartialEntity() : base()
        {
        }

        public UserPartialEntity(string empCode) : this()
        {
            object _ = ValidateNew(empCode, out Exception ex) ? null : throw ex;

            EmpCode = empCode;

            QueuePipelineEvent(new UserCreatedEvent(this));
        }

        #region Validation rules
        private bool ValidateNew(string empCode, out Exception ex)
        {
            List<string> invalidFields = new List<string>();

            if (string.IsNullOrWhiteSpace(empCode))
            {
                invalidFields.Add(nameof(EmpCode));
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
