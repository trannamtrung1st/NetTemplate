using FluentValidation;
using NetTemplate.Blog.ApplicationCore.Comment.Models;

namespace NetTemplate.Blog.ApplicationCore.Comment.Validators
{
    public class UpdateCommentModelValidator : AbstractValidator<UpdateCommentModel>
    {
        public UpdateCommentModelValidator(IValidator<BaseCommentModel> baseValidator)
        {
            Include(baseValidator);
        }
    }
}
