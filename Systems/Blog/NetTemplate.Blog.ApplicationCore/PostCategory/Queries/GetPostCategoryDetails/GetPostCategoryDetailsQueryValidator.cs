using FluentValidation;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Queries.GetPostCategoryDetails
{
    public class GetPostCategoryDetailsQueryValidator : AbstractValidator<GetPostCategoryDetailsQuery>
    {
        public GetPostCategoryDetailsQueryValidator()
        {
            RuleFor(e => e.Id).GreaterThan(0);
        }
    }
}
