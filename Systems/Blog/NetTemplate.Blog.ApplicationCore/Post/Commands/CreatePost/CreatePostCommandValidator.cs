using FluentValidation;
using NetTemplate.Blog.ApplicationCore.Post.Models;

namespace NetTemplate.Blog.ApplicationCore.Post.Commands.CreatePost
{
    public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator(IValidator<CreatePostModel> createPostModelValidator)
        {
            RuleFor(e => e.Model).NotNull().SetValidator(createPostModelValidator);
        }
    }
}
