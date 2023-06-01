using FluentValidation;
using NetTemplate.Blog.ApplicationCore.User.Models;

namespace NetTemplate.Blog.ApplicationCore.User.Queries.GetUsers
{
    public class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
    {
        public GetUsersQueryValidator(IValidator<UserListRequestModel> requestValidator)
        {
            RuleFor(e => e.Model).NotNull()
                .SetValidator(requestValidator);
        }
    }
}
