using CommonConstraints = NetTemplate.Shared.ApplicationCore.Common.Constants.Constraints;

namespace NetTemplate.Shared.ApplicationCore.Common.Utils
{
    public static class ApplicationValidation
    {
        public static bool ValidateMaxLength(IEnumerable<string> values,
            int maxLength = CommonConstraints.MaxStringLength)
            => values.All(val => !(val?.Length > maxLength));
    }
}
