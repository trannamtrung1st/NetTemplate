using FluentValidation;
using NetTemplate.Blog.ApplicationCore.Post.Models;

namespace NetTemplate.Blog.ApplicationCore.Post.Validators
{
    public class UpdatePostTagsModelValidator : AbstractValidator<UpdatePostTagsModel>
    {
        public UpdatePostTagsModelValidator()
        {
            RuleFor(e => e.Tags).NotNull();
        }
    }
}
