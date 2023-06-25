namespace NetTemplate.Identity.Infrastructure.PubSub.Models
{
    public class PubSubConfig
    {
        public bool UseKafka { get; set; }
        public bool UseRedis { get; set; }


        public const string ConfigurationSection = nameof(PubSubConfig);
    }
}
