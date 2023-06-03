using NetTemplate.Blog.ApplicationCore.PostCategory.Interfaces;
using NetTemplate.Common.DependencyInjection;
using NetTemplate.Shared.ApplicationCore.Common.Exceptions;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Implementations
{
    [ScopedService]
    public class PostCategoryValidator : IPostCategoryValidator
    {
        private readonly IPostCategoryRepository _postCategoryRepository;

        public PostCategoryValidator(IPostCategoryRepository postCategoryRepository)
        {
            _postCategoryRepository = postCategoryRepository;
        }

        public async Task ValidatePostCategoryName(string name, CancellationToken cancellationToken = default)
        {
            bool exists = await _postCategoryRepository.NameExists(name, cancellationToken);

            if (exists) throw new BusinessException(ResultCodes.PostCategory.NameAlreadyExists);
        }
    }
}
