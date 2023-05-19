using NetTemplate.Blog.ApplicationCore.User.Events;
using NetTemplate.Common.Validation;
using NetTemplate.Shared.ApplicationCore.Constants;
using NetTemplate.Shared.ApplicationCore.Entities;
using NetTemplate.Shared.ApplicationCore.Exceptions;
using NetTemplate.Shared.ApplicationCore.Utils;

namespace NetTemplate.Blog.ApplicationCore.User
{
    public class UserPartialEntity : AppFullAuditableEntity<int>, IAggregateRoot
    {
        public string UserCode { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public bool Active { get; private set; }

        protected UserPartialEntity() : base()
        {
        }

        public UserPartialEntity(string userCode, string fisrtName, string lastName, bool isActive) : this()
        {
            object _ = ValidateNew(userCode, fisrtName, lastName, isActive, out Exception ex) ? null : throw ex;

            UserCode = userCode;
            FirstName = fisrtName;
            LastName = lastName;
            Active = isActive;

            QueuePipelineEvent(new UserCreatedEvent(this));
        }

        public void SetActive(bool isActive)
        {
            if (Active != isActive)
            {
                Active = isActive;

                // [NOTE] emit event if necessary
            }
        }

        public void UpdateInfo(string firstName, string lastName)
        {
            object _ = ValidateUpdateInfo(firstName, lastName, out Exception ex) ? null : throw ex;

            FirstName = firstName;
            LastName = lastName;

            // [NOTE] emit event
        }

        #region Validation rules

        private bool ValidateNew(string userCode, string firstName, string lastName, bool isActive, out Exception ex)
        {
            List<string> invalidFields = new List<string>();

            if (string.IsNullOrWhiteSpace(userCode))
            {
                invalidFields.Add(nameof(UserCode));
            }

            invalidFields.AddRange(ValidateUserInfo(firstName, lastName));

            ex = invalidFields.Count > 0 ? new InvalidEntityDataException(invalidFields.ToArray()) : null;

            return ex == null;
        }

        private bool ValidateUpdateInfo(string firstName, string lastName, out Exception ex)
        {
            List<string> invalidFields = new List<string>();

            invalidFields.AddRange(ValidateUserInfo(firstName, lastName));

            ex = invalidFields.Count > 0 ? new InvalidEntityDataException(invalidFields.ToArray()) : null;

            return ex == null;
        }

        private List<string> ValidateUserInfo(string firstName, string lastName)
        {
            List<string> invalidFields = new List<string>();

            if (!ApplicationValidation.ValidateMaxLength(new[] { firstName, lastName }))
            {
                invalidFields.Add(Messages.Common.InvalidMaxLength);
            }

            if (!CommonValidation.StringNotEmptyOrWhitespace(new[] { firstName, lastName }))
            {
                invalidFields.Add(Messages.Common.MissingRequiredFields);
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
