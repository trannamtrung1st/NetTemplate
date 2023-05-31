namespace AutoMapper
{
    public static class MapperExtensions
    {
        public static IQueryable<TResult> CustomProjectTo<TResult>(this IMapper mapper, IQueryable query)
        {
            if (typeof(TResult) == query.ElementType)
            {
                return (IQueryable<TResult>)query;
            }

            return mapper.ProjectTo<TResult>(query);
        }
    }
}
