using FluentValidation;
using NetTemplate.Blog.ApplicationCore.Comment.Models;

namespace NetTemplate.Blog.ApplicationCore.Comment.Validators
{
    public class CreateCommentModelValidator : AbstractValidator<CreateCommentModel>
    {
        public CreateCommentModelValidator(IValidator<BaseCommentModel> baseValidator)
        {
            Include(baseValidator);
        }
    }
}
