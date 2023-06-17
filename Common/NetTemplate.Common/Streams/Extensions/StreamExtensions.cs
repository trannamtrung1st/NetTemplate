namespace NetTemplate.Common.Streams.Extensions
{
    public static class StreamExtensions
    {
        public static Task<string> ReadAsStringAsync(this Stream stream)
        {
            var reader = new StreamReader(stream);
            return reader.ReadToEndAsync();
        }
    }
}
