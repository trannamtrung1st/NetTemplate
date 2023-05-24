namespace NetTemplate.Shared.WebApi.Models
{
    public class SerilogConfig
    {
        public long MaxBodyLengthForLogging { get; set; }

        private SerilogConfig() { }

        private static SerilogConfig _instance;
        public static SerilogConfig Instance
        {
            get
            {
                if (_instance == null) _instance = new SerilogConfig();
                return _instance;
            }
        }
    }
}
