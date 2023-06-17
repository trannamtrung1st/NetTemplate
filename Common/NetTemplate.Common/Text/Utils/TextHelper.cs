namespace NetTemplate.Common.Text.Utils
{
    public static class TextHelper
    {
        public static string Base64Encode(byte[] data)
        {
            return Convert.ToBase64String(data);
        }

        public static byte[] Base64Decode(string base64)
        {
            return Convert.FromBase64String(base64);
        }
    }
}
