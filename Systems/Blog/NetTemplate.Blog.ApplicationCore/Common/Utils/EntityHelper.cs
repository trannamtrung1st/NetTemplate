using NetTemplate.Blog.ApplicationCore.Common.Interfaces;
using System.Linq.Expressions;

namespace NetTemplate.Blog.ApplicationCore.Common.Utils
{
    public static class EntityHelper
    {
        public static Expression<Func<T, string>> GetCreatorFullNameExpression<T>() where T : IHasCreator
            => (e) => e.Creator.Id > 0 ? e.Creator.FirstName + " " + e.Creator.LastName : null;
    }
}
