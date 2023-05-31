using FluentValidation;
using NetTemplate.Blog.ApplicationCore.Comment.Models;

namespace NetTemplate.Blog.ApplicationCore.Comment.Validators
{
    public class CommentListRequestModelValidator : AbstractValidator<CommentListRequestModel>
    {
        public CommentListRequestModelValidator()
        {
        }
    }
}
