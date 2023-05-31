using FluentValidation;
using NetTemplate.Blog.ApplicationCore.Comment.Models;

namespace NetTemplate.Blog.ApplicationCore.Comment.Queries.GetPostComments
{
    public class GetPostCommentsQueryValidator : AbstractValidator<GetPostCommentsQuery>
    {
        public GetPostCommentsQueryValidator(IValidator<CommentListRequestModel> requestValidator)
        {
            RuleFor(e => e.OnPostId).GreaterThan(0);

            RuleFor(e => e.Model).NotNull()
                .SetValidator(requestValidator);
        }
    }
}
