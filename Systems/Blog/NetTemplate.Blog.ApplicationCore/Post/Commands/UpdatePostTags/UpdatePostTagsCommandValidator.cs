using FluentValidation;
using NetTemplate.Blog.ApplicationCore.Post.Models;

namespace NetTemplate.Blog.ApplicationCore.Post.Commands.UpdatePostTags
{
    public class UpdatePostTagsCommandValidator : AbstractValidator<UpdatePostTagsCommand>
    {
        public UpdatePostTagsCommandValidator(IValidator<UpdatePostTagsModel> updatePostTagsModelValidator)
        {
            RuleFor(e => e.Id).GreaterThan(0);

            RuleFor(e => e.Model).NotNull().SetValidator(updatePostTagsModelValidator);
        }
    }
}
