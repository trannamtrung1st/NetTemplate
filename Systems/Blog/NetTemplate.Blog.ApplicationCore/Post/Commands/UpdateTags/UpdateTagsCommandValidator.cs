using FluentValidation;
using NetTemplate.Blog.ApplicationCore.Post.Models;

namespace NetTemplate.Blog.ApplicationCore.Post.Commands.UpdateTags
{
    public class UpdateTagsCommandValidator : AbstractValidator<UpdateTagsCommand>
    {
        public UpdateTagsCommandValidator(IValidator<UpdatePostTagsModel> updatePostTagsModelValidator)
        {
            RuleFor(e => e.Model).NotNull().SetValidator(updatePostTagsModelValidator);
        }
    }
}
