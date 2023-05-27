using FluentValidation;
using NetTemplate.Blog.ApplicationCore.Post.Models;

namespace NetTemplate.Blog.ApplicationCore.Post.Queries.GetPosts
{
    public class GetPostsQueryValidator : AbstractValidator<GetPostsQuery>
    {
        public GetPostsQueryValidator(IValidator<PostListRequestModel> requestValidator)
        {
            RuleFor(e => e.Model).NotNull()
                .SetValidator(requestValidator);
        }
    }
}
