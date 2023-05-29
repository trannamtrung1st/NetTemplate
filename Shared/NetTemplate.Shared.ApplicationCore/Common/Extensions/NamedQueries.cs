using NetTemplate.Common.Enumerations;
using NetTemplate.Shared.ApplicationCore.Common.Entities;
using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace System.Linq
{
    public static partial class NamedQueries
    {
        public static IQueryable<T> IsNotDeleted<T>(this IQueryable<T> query) where T : ISoftDeleteEntity
        {
            return query.Where(e => !e.IsDeleted);
        }

        public static IQueryable<T> ById<T, TId>(this IQueryable<T> query, TId id)
            where T : IHasId<TId>
        {
            return query.Where(e => Equals(e.Id, id));
        }

        public static IQueryable<T> ByIds<T, TId>(this IQueryable<T> query, IEnumerable<TId> ids)
            where T : IHasId<TId>
        {
            return query.Where(e => ids.Contains(e.Id));
        }

        public static IQueryable<T> ByIdsIfAny<T, TId>(this IQueryable<T> query, IEnumerable<TId> ids)
            where T : IHasId<TId>
        {
            if (ids?.Any() == true)
            {
                return ByIds(query, ids);
            }

            return query;
        }

        public static IQueryable<T> Paging<T, TModel>(this IQueryable<T> query, TModel model)
            where TModel : IPagingQuery
        {
            query = query.Skip(model.Skip);

            if (!model.CanGetAll() || model.Take != null)
            {
                query = query.Take(model.GetTakeOrDefault());
            }

            return query;
        }

        /// <summary>
        /// [NOTE] If Process return null, default will be applied
        /// </summary>
        public static IQueryable<T> SortBy<T, TSortBy>(this IQueryable<T> query, TSortBy[] sortBy, bool[] isDesc,
            Func<IQueryable<T>, TSortBy, bool, IQueryable<T>> Process = null,
            Func<TSortBy, bool> IsUseColumn = null,
            string lastSortDefault = "Id",
            bool lastSortDefaultDesc = false)
            where TSortBy : struct, Enum
        {
            if (sortBy != null)
            {
                for (int i = 0; i < sortBy.Length; i++)
                {
                    TSortBy currentSort = sortBy[i];
                    bool currentIsDesc = isDesc[i];
                    IQueryable<T> processedQuery = null;

                    if (Process != null)
                    {
                        processedQuery = Process(query, currentSort, currentIsDesc);
                    }

                    if (processedQuery == null)
                    {
                        string columnName;

                        if (IsUseColumn == null || IsUseColumn(currentSort))
                        {
                            columnName = currentSort.GetColumn() ?? currentSort.GetName();
                        }
                        else
                        {
                            columnName = currentSort.GetName();
                        }

                        processedQuery = query.SortSequential(columnName, currentIsDesc);
                    }

                    query = processedQuery;
                }
            }

            if (lastSortDefault != null)
            {
                query = query.SortSequential(lastSortDefault, lastSortDefaultDesc);
            }

            return query;
        }
    }
}
