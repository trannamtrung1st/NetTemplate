namespace System
{
    public static class DateTimeExtensions
    {
        public static string ToHttpDateString(this DateTime dateTime)
            => dateTime.ToUniversalTime().ToString("r");

        public static DateTime ToLocalFromTimeZone(this DateTime dateTime, TimeZoneInfo timeZone)
        {
            return DateTime.SpecifyKind(
                    TimeZoneInfo.ConvertTime(dateTime, timeZone, TimeZoneInfo.Local),
                DateTimeKind.Local);
        }
    }
}
