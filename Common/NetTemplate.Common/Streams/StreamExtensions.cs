namespace NetTemplate.Common.Streams
{
    public static class StreamExtensions
    {
        public static Task<string> ReadAsStringAsync(this Stream stream)
        {
            using var reader = new StreamReader(stream);
            return reader.ReadToEndAsync();
        }
    }
}
