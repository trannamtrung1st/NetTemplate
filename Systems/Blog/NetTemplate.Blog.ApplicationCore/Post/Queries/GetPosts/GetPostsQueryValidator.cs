using FluentValidation;

namespace NetTemplate.Blog.ApplicationCore.Post.Queries.GetPosts
{
    public class GetPostsQueryValidator : AbstractValidator<GetPostsQuery>
    {
        public GetPostsQueryValidator()
        {
            RuleFor(e => e.Model).NotNull();
        }
    }
}
