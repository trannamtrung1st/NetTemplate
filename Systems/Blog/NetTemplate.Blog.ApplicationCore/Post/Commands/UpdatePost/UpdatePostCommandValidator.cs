using FluentValidation;
using NetTemplate.Blog.ApplicationCore.Post.Models;

namespace NetTemplate.Blog.ApplicationCore.Post.Commands.UpdatePost
{
    public class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
    {
        public UpdatePostCommandValidator(IValidator<UpdatePostModel> updatePostModelValidator)
        {
            RuleFor(e => e.Id).GreaterThan(0);

            RuleFor(e => e.Model).NotNull().SetValidator(updatePostModelValidator);
        }
    }
}
