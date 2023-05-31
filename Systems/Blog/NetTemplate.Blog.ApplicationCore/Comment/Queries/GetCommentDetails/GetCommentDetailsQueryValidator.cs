using FluentValidation;

namespace NetTemplate.Blog.ApplicationCore.Comment.Queries.GetCommentDetails
{
    public class GetCommentDetailsQueryValidator : AbstractValidator<GetCommentDetailsQuery>
    {
        public GetCommentDetailsQueryValidator()
        {
            RuleFor(e => e.Id).GreaterThan(0);
        }
    }
}
