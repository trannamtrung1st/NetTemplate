using FluentValidation;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Commands.DeletePostCategory
{
    public class DeletePostCategoryCommandValidator : AbstractValidator<DeletePostCategoryCommand>
    {
        public DeletePostCategoryCommandValidator()
        {
            RuleFor(e => e.Id).GreaterThan(0);
        }
    }
}
