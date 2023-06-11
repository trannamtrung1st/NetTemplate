namespace NetTemplate.Blog.Infrastructure.PubSub.Models
{
    public class PubSubConfig
    {
        public bool UseKafka { get; set; }
        public bool UseRedis { get; set; }
    }
}
