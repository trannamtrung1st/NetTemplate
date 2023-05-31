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
    public class UserPartialRepository : EFCoreRepository<UserPartialEntity, MainDbContext>, IUserPartialRepository
    {
        public UserPartialRepository(MainDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<UserPartialEntity> FindByCode(string userCode)
        {
            UserPartialEntity entity = await DbSet.Where(e => e.UserCode == userCode).FirstOrDefaultAsync();

            if (entity != null)
            {
                await LoadAggregate(entity);
            }

            return entity;
        }

        public async Task<QueryResponseModel<TResult>> Query<TResult>(
            string terms = null,
            IEnumerable<string> userCodes = null,
            IEnumerable<int> ids = null,
            IOffsetPagingQuery paging = null,
            bool count = true)
        {
            IQueryable<UserPartialEntity> query = DbSet;

            if (!string.IsNullOrEmpty(terms))
            {
                query = query.Where(e => e.UserCode.Contains(terms)
                    || e.FirstName.Contains(terms)
                    || e.LastName.Contains(terms));
            }

            query = query.ByIdsIfAny(ids);

            if (userCodes?.Any() == true)
            {
                query = query.Where(u => userCodes.Contains(u.UserCode));
            }

            int? total = null;

            if (count)
            {
                total = await query.CountAsync();
            }

            query = query.OffsetPaging(paging);

            IQueryable<TResult> result = mapper.CustomProjectTo<TResult>(query);

            return new QueryResponseModel<TResult>(total, result);
        }

        public override Task<IQueryable<TResult>> QueryById<TResult>(params object[] keys)
            => QueryById<UserPartialEntity, TResult, int>(keys);

        protected override Task LoadAggregate(UserPartialEntity entity)
        {
            return Task.CompletedTask;
        }
    }
}
