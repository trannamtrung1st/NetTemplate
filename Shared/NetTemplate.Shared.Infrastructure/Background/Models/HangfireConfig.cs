using TimeZoneConverter;

namespace NetTemplate.Shared.Infrastructure.Background.Models
{
    public class HangfireConfig
    {
        public string ServerName { get; set; }
        public bool UseDashboard { get; set; }
        public string TimeZone { get; set; }
        public string DatabaseName { get; set; }
        public TimeZoneInfo TimeZoneInfo => TimeZone != null ? TZConvert.GetTimeZoneInfo(TimeZone) : null;
        public IEnumerable<CronJob> Jobs { get; set; }
    }
}
