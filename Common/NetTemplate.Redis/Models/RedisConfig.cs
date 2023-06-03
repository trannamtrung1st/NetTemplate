namespace NetTemplate.Redis.Models
{
    public class RedisConfig
    {
        public string User { get; set; }
        public string Password { get; set; }
        public IEnumerable<string> Endpoints { get; set; }
    }
}
