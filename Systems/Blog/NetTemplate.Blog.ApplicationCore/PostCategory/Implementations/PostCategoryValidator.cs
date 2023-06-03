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

        public async Task ValidatePostCategoryName(string currentName, string newName, CancellationToken cancellationToken = default)
        {
            if (currentName == newName) return;

            bool exists = await _postCategoryRepository.NameExists(newName, cancellationToken);

            if (exists) throw new BusinessException(ResultCodes.PostCategory.NameAlreadyExists);
        }
    }
}
