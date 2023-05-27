using FluentValidation;
using NetTemplate.Blog.ApplicationCore.PostCategory.Models;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Queries.GetPostCategories
{
    public class GetPostCategoriesQueryValidator : AbstractValidator<GetPostCategoriesQuery>
    {
        public GetPostCategoriesQueryValidator(IValidator<PostCategoryListRequestModel> requestValidator)
        {
            RuleFor(e => e.Model).NotNull()
                .SetValidator(requestValidator);
        }
    }
}
