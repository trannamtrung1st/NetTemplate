using NetTemplate.Shared.ApplicationCore.Constants;

namespace NetTemplate.Shared.ApplicationCore.Utils
{
    public static class ApplicationValidation
    {
        public static bool ValidateMaxLength(IEnumerable<string> values,
            int maxLength = Constraints.Common.MaxStringLength)
            => values.All(val => !(val?.Length > maxLength));
    }
}
