using FluentValidation;
using NetTemplate.Blog.ApplicationCore.Post.Models;

namespace NetTemplate.Blog.ApplicationCore.Post.Validators
{
    public class UpdatePostModelValidator : AbstractValidator<UpdatePostModel>
    {
        public UpdatePostModelValidator(IValidator<BasePostModel> baseValidator)
        {
            Include(baseValidator);
        }
    }
}
