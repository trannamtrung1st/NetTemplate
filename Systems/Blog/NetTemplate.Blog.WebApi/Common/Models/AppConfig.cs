namespace NetTemplate.Blog.WebApi.Common.Models
{
    // [IMPORTANT] should only use this as arguments if possible
    public class AppConfig
    {
        public int ResponseCacheTtl { get; set; }
        public bool DbContextDebugEnabled { get; set; }

        private AppConfig() { }

        private static AppConfig _instance;
        public static AppConfig Instance
        {
            get
            {
                if (_instance == null) _instance = new AppConfig();
                return _instance;
            }
        }
    }
}
