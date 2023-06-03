using NetTemplate.Blog.ApplicationCore.Post.Interfaces;
using NetTemplate.Blog.ApplicationCore.PostCategory;
using NetTemplate.Common.DependencyInjection;
using NetTemplate.Shared.ApplicationCore.Common.Exceptions;

namespace NetTemplate.Blog.ApplicationCore.Post.Implementations
{
    [ScopedService]
    public class PostValidator : IPostValidator
    {
        private readonly IPostRepository _postRepository;
        private readonly IPostCategoryRepository _postCategoryRepository;

        public PostValidator(IPostRepository postRepository,
            IPostCategoryRepository postCategoryRepository)
        {
            _postRepository = postRepository;
            _postCategoryRepository = postCategoryRepository;
        }

        public async Task ValidateExistences(int categoryId, CancellationToken cancellationToken = default)
        {
            bool categoryExists = await _postCategoryRepository.Exists(categoryId);

            if (!categoryExists) throw new NotFoundException(nameof(PostCategoryEntity));
        }

        public async Task ValidatePostTitle(string title, CancellationToken cancellationToken = default)
        {
            bool exists = await _postRepository.TitleExists(title, cancellationToken);

            if (exists) throw new BusinessException(ResultCodes.Post.TitleAlreadyExists);
        }
    }
}
