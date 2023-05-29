using FluentValidation;

namespace NetTemplate.Blog.ApplicationCore.Post.Queries.GetPostDetails
{
    public class GetPostDetailsQueryValidator : AbstractValidator<GetPostDetailsQuery>
    {
        public GetPostDetailsQueryValidator()
        {
            RuleFor(e => e.Id).GreaterThan(0);
        }
    }
}
