﻿using NetTemplate.Shared.ApplicationCore.Common.Interfaces;
using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Blog.ApplicationCore.User
{
    public interface IUserPartialRepository : IRepository<UserPartialEntity, int>
    {
        Task<QueryResponseModel<TResult>> Query<TResult>(
            string terms = null,
            IEnumerable<string> userCodes = null,
            IEnumerable<int> ids = null,
            bool? active = null,
            Enums.UserSortBy[] sortBy = null,
            bool[] isDesc = null,
            IOffsetPagingQuery paging = null,
            bool count = true,
            CancellationToken cancellationToken = default);

        Task<UserPartialEntity> FindByCode(string userCode, CancellationToken cancellationToken = default);
    }
}
