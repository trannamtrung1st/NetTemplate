namespace NetTemplate.Shared.Infrastructure.Persistence.Models
{
    public class DatabaseConfig
    {
        public bool EnableDebug { get; set; }

        private DatabaseConfig() { }

        private static DatabaseConfig _instance;
        public static DatabaseConfig Instance
        {
            get
            {
                if (_instance == null) _instance = new DatabaseConfig();
                return _instance;
            }
        }
    }
}
