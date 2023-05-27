using FluentValidation;
using NetTemplate.Blog.ApplicationCore.PostCategory.Models;
using NetTemplate.Shared.ApplicationCore.Common.Utils;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Validators
{
    public class PostCategoryListRequestModelValidator : AbstractValidator<PostCategoryListRequestModel>
    {
        public PostCategoryListRequestModelValidator()
        {
            this.ValidateSortableQuery<PostCategoryListRequestModel, Enums.PostCategorySortBy>();
        }
    }
}
