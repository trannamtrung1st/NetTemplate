namespace NetTemplate.Shared.WebApi.Common.Models
{
    public class WebInfoConfig
    {
        public string ApiTitle { get; set; }
        public string ApiDescription { get; set; }


        public const string ConfigurationSection = nameof(WebInfoConfig);
    }
}
