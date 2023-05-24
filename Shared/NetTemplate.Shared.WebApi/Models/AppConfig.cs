namespace NetTemplate.Shared.WebApi.Models
{
    public class AppConfig
    {
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
