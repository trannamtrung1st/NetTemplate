using NetTemplate.Shared.ApplicationCore.Common.Interfaces;
using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Blog.ApplicationCore.User
{
    public interface IUserPartialRepository : IRepository<UserPartialEntity>
    {
        Task<QueryResponseModel<TResult>> Query<TResult>(
            string terms = null,
            IEnumerable<string> userCodes = null,
            IEnumerable<int> ids = null,
            bool? active = null,
            Enums.UserSortBy[] sortBy = null,
            bool[] isDesc = null,
            IOffsetPagingQuery paging = null,
            bool count = true);

        Task<UserPartialEntity> FindByCode(string userCode);
    }
}
