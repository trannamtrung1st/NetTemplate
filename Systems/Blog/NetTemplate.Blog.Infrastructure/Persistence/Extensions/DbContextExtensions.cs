using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace NetTemplate.Blog.Infrastructure.Persistence.Extensions
{
    internal static class DbContextExtensions
    {
        public static bool TryAttach<T>(this DbContext dbContext, T entity, out EntityEntry<T> entry)
            where T : class
        {
            entry = dbContext.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                entry = dbContext.Attach(entity);
                return true;
            }

            return false;
        }

        public static EntityEntry<E> Update<E>(this DbContext dbContext,
            E entity, params Expression<Func<E, object>>[] changedProperties)
            where E : class
        {
            EntityEntry<E> entry;
            dbContext.TryAttach(entity, out entry);

            if (changedProperties?.Any() == true)
            {
                foreach (var property in changedProperties)
                    entry.Property(property).IsModified = true;
            }
            else return dbContext.Update(entity);

            return entry;
        }
    }
}
