namespace NetTemplate.Shared.WebApi.Models
{
    public class WebApiConfig
    {
        public string ApiTitle { get; set; }
        public string ApiDescription { get; set; }

        private WebApiConfig() { }

        private static WebApiConfig _instance;
        public static WebApiConfig Instance
        {
            get
            {
                if (_instance == null) _instance = new WebApiConfig();
                return _instance;
            }
        }
    }
}
