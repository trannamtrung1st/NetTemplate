using FluentValidation;
using NetTemplate.Blog.ApplicationCore.Comment.Models;

namespace NetTemplate.Blog.ApplicationCore.Comment.Commands.UpdateComment
{
    public class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
    {
        public UpdateCommentCommandValidator(IValidator<UpdateCommentModel> updateCommentModelValidator)
        {
            RuleFor(e => e.Id).GreaterThan(0);

            RuleFor(e => e.Model).NotNull().SetValidator(updateCommentModelValidator);
        }
    }
}
