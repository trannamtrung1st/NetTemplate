namespace NetTemplate.Shared.Infrastructure.Background.Models
{
    public class CronJob
    {
        public bool Enabled { get; set; }
        public IEnumerable<string> CronExpressions { get; set; }
        public string Name { get; set; }
        public IDictionary<string, object> JobData { get; set; }
    }
}
