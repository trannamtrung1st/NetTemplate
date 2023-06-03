using NetTemplate.Blog.ApplicationCore.Comment.Interfaces;
using NetTemplate.Blog.ApplicationCore.Post;
using NetTemplate.Shared.ApplicationCore.Common.Exceptions;

namespace NetTemplate.Blog.ApplicationCore.Comment.Implementations
{
    public class CommentValidator : ICommentValidator
    {
        private readonly IPostRepository _postRepository;

        public CommentValidator(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task ValidateExistences(int postId, CancellationToken cancellationToken = default)
        {
            bool postExists = await _postRepository.Exists(postId);

            if (!postExists) throw new NotFoundException(nameof(PostEntity));
        }
    }
}
