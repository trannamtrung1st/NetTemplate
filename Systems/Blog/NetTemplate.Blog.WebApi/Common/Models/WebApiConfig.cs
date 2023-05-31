using NetTemplate.Blog.Infrastructure.Common.Models;

namespace NetTemplate.Blog.WebApi.Common.Models
{
    // [IMPORTANT] should only use this as arguments if possible
    public class WebApiConfig : InfrastructureConfig
    {
        public int ResponseCacheTtl { get; set; }

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
