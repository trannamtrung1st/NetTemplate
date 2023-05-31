using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetTemplate.Blog.ApplicationCore.Comment;
using NetTemplate.Blog.Infrastructure.Persistence;
using NetTemplate.Common.DependencyInjection;
using NetTemplate.Shared.ApplicationCore.Common.Models;
using NetTemplate.Shared.Infrastructure.Persistence.Repositories;

namespace NetTemplate.Blog.Infrastructure.Domains.Comment
{
    [ScopedService]
    public class CommentRepository : EFCoreRepository<CommentEntity, MainDbContext>, ICommentRepository
    {
        public CommentRepository(MainDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<QueryResponseModel<TResult>> Query<TResult>(int onPostId,
            IEnumerable<int> ids = null,
            int? creatorId = null,
            Enums.CommentSortBy[] sortBy = null,
            bool[] isDesc = null,
            IKeySetPagingQuery<DateTimeOffset> paging = null,
            bool count = true)
        {
            IQueryable<CommentEntity> query = DbSet.OnPost(onPostId);

            if (creatorId != null)
            {
                query = query.CreatedBy(creatorId.Value);
            }

            query = query.ByIdsIfAny(ids);

            int? total = null;

            if (count)
            {
                total = await query.CountAsync();
            }

            query = query.SortBy(sortBy, isDesc);

            query = query.KeySetPaging(paging);

            IQueryable<TResult> result = mapper.CustomProjectTo<TResult>(query);

            return new QueryResponseModel<TResult>(total, result);
        }

        public override Task<IQueryable<TResult>> QueryById<TResult>(params object[] keys)
            => QueryById<CommentEntity, TResult, int>(keys);

        protected override Task LoadAggregate(CommentEntity entity)
        {
            return Task.CompletedTask;
        }
    }
}
