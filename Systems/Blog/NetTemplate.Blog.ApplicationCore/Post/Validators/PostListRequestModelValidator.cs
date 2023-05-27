using FluentValidation;
using NetTemplate.Blog.ApplicationCore.Post.Models;
using NetTemplate.Shared.ApplicationCore.Common.Utils;

namespace NetTemplate.Blog.ApplicationCore.Post.Validators
{
    public class PostListRequestModelValidator : AbstractValidator<PostListRequestModel>
    {
        public PostListRequestModelValidator()
        {
            this.ValidateSortableQuery<PostListRequestModel, Enums.PostSortBy>();
        }
    }
}
