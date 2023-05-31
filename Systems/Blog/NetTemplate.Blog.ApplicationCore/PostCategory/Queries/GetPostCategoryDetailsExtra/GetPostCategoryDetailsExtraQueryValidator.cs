using FluentValidation;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Queries.GetPostCategoryDetailsExtra
{
    public class GetPostCategoryDetailsExtraQueryValidator : AbstractValidator<GetPostCategoryDetailsExtraQuery>
    {
        public GetPostCategoryDetailsExtraQueryValidator()
        {
            RuleFor(e => e.Id).GreaterThan(0);
        }
    }
}
