using FluentValidation;
using NetTemplate.Blog.ApplicationCore.User.Models;
using NetTemplate.Shared.ApplicationCore.Common.Utils;

namespace NetTemplate.Blog.ApplicationCore.User.Validators
{
    public class UserListRequestModelValidator : AbstractValidator<UserListRequestModel>
    {
        public UserListRequestModelValidator()
        {
            this.ValidateSortableQuery<UserListRequestModel, Enums.UserSortBy>();
        }
    }
}
