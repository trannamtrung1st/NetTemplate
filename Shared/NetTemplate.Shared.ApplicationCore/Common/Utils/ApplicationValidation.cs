using FluentValidation;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;
using CommonConstraints = NetTemplate.Shared.ApplicationCore.Common.Constants.Constraints;
using CommonMessages = NetTemplate.Shared.ApplicationCore.Common.Constants.Messages;

namespace NetTemplate.Shared.ApplicationCore.Common.Utils
{
    public static class ApplicationValidation
    {
        public static bool ValidateMaxLength(IEnumerable<string> values,
            int maxLength = CommonConstraints.MaxStringLength)
            => values.All(val => !(val?.Length > maxLength));

        public static IConditionBuilder ValidateSortableQuery<T, TSortBy>(this AbstractValidator<T> validator)
            where T : ISortableQuery<TSortBy>
        {
            return validator.When(e => e.SortBy?.Length > 0 == true, () =>
            {
                validator.RuleFor(e => e.IsDesc)
                    .NotEmpty()
                    .Must((e, val) => val?.Length == e.SortBy.Length)
                    .WithMessage(CommonMessages.IsDescAndSortByMismatch);
            });
        }
    }
}
