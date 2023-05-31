using NetTemplate.Shared.ApplicationCore.Common.Interfaces;
using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Blog.ApplicationCore.User
{
    public interface IUserPartialRepository : IRepository<UserPartialEntity>
    {
        Task<QueryResponseModel<UserPartialEntity>> Query(
            string terms = null,
            IEnumerable<string> userCodes = null,
            IEnumerable<int> ids = null,
            IPagingQuery paging = null,
            bool count = true);

        Task<UserPartialEntity> FindByCode(string userCode);
    }
}
