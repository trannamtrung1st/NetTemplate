using NetTemplate.Shared.ApplicationCore.Entities;
using NetTemplate.Shared.ApplicationCore.Exceptions;

namespace NetTemplate.Blog.ApplicationCore.Post
{
    public class PostTagEntity : AppAuditableEntity<int>
    {
        public string Value { get; private set; }

        public PostTagEntity(string value)
        {
            object _ = ValidateNew(value, out Exception ex) ? null : throw ex;

            Value = value;
        }


        #region Validation rules

        private bool ValidateNew(string value, out Exception ex)
        {
            List<string> invalidFields = new List<string>();

            if (string.IsNullOrWhiteSpace(value) || value.Length > Constraints.MaxValueLength)
            {
                invalidFields.Add(nameof(Value));
            }

            ex = invalidFields.Count > 0 ? new InvalidEntityDataException(invalidFields.ToArray()) : null;

            return ex == null;
        }

        #endregion

        public override int TransientIdValue() => default;

        public static class Constraints
        {
            public const int MaxValueLength = 50;
        }
    }
}
