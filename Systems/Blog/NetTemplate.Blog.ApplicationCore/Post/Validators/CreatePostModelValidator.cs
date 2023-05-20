using FluentValidation;
using NetTemplate.Blog.ApplicationCore.Post.Models;

namespace NetTemplate.Blog.ApplicationCore.Post.Validators
{
    public class CreatePostModelValidator : AbstractValidator<CreatePostModel>
    {
        public CreatePostModelValidator(IValidator<BasePostModel> baseValidator)
        {
            Include(baseValidator);
        }
    }
}
