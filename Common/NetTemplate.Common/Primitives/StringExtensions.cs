using System.Globalization;
using System.Text;

namespace System
{
    public static class StringExtensions
    {
        public static string RemoveAccents(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static MemoryStream ToStream(this string strValue)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(strValue));
        }
    }
}
