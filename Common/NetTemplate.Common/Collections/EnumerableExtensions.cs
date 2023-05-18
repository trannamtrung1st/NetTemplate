namespace System.Linq
{
    public static class EnumerableExtensions
    {
        public static T[] SubSet<T>(this T[] arr, int fromIdx, int length)
        {
            var newArr = new T[length];

            Array.Copy(arr, fromIdx, newArr, 0, length);

            return newArr;
        }

        public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> items,
            CancellationToken cancellationToken = default)
        {
            var results = new List<T>();

            await foreach (var item in items.WithCancellation(cancellationToken))
                results.Add(item);

            return results;
        }
    }
}
