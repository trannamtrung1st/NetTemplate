using NetTemplate.Shared.ApplicationCore.Common.Constants;

namespace NetTemplate.Shared.ApplicationCore.Common.Utils
{
    public static class ApplicationValidation
    {
        public static bool ValidateMaxLength(IEnumerable<string> values,
            int maxLength = Constraints.Common.MaxStringLength)
            => values.All(val => !(val?.Length > maxLength));
    }
}
