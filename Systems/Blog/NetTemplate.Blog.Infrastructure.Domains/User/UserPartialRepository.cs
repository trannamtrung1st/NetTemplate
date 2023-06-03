using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetTemplate.Blog.ApplicationCore.User;
using NetTemplate.Blog.Infrastructure.Persistence;
using NetTemplate.Common.DependencyInjection;
using NetTemplate.Shared.ApplicationCore.Common.Models;
using NetTemplate.Shared.Infrastructure.Persistence.Repositories;

namespace NetTemplate.Blog.Infrastructure.Domains.User
{
    [ScopedService]
    public class UserPartialRepository : EFCoreRepository<UserPartialEntity, int, MainDbContext>, IUserPartialRepository
    {
        public UserPartialRepository(MainDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<UserPartialEntity> FindByCode(string userCode, CancellationToken cancellationToken = default)
        {
            UserPartialEntity entity = await DbSet.Where(e => e.UserCode == userCode).FirstOrDefaultAsync(cancellationToken);

            if (entity != null)
            {
                await LoadAggregate(entity, cancellationToken);
            }

            return entity;
        }

        public async Task<QueryResponseModel<TResult>> Query<TResult>(
            string terms = null,
            IEnumerable<string> userCodes = null,
            IEnumerable<int> ids = null,
            bool? active = null,
            Enums.UserSortBy[] sortBy = null,
            bool[] isDesc = null,
            IOffsetPagingQuery paging = null,
            bool count = true,
            CancellationToken cancellationToken = default)
        {
            IQueryable<UserPartialEntity> query = DbSet;

            if (!string.IsNullOrEmpty(terms))
            {
                query = query.Where(e => e.UserCode.Contains(terms)
                    || e.FirstName.Contains(terms)
                    || e.LastName.Contains(terms));
            }

            if (userCodes?.Any() == true)
            {
                query = query.Where(e => userCodes.Contains(e.UserCode));
            }

            if (active != null)
            {
                query = query.Where(e => e.Active == active);
            }

            query = query.ByIdsIfAny(ids);

            if (userCodes?.Any() == true)
            {
                query = query.Where(u => userCodes.Contains(u.UserCode));
            }

            int? total = null;

            if (count)
            {
                total = await query.CountAsync(cancellationToken);
            }

            query = query.SortBy(sortBy, isDesc);

            query = query.OffsetPaging(paging);

            IQueryable<TResult> result = mapper.CustomProjectTo<TResult>(query);

            return new QueryResponseModel<TResult>(total, result);
        }

        protected override Task LoadAggregate(UserPartialEntity entity, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
