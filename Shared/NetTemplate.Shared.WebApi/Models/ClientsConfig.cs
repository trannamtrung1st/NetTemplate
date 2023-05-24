namespace NetTemplate.Shared.WebApi.Models
{
    public class ClientsConfig
    {
        public IEnumerable<ApplicationClient> Clients { get; set; }

        private ClientsConfig() { }

        private static ClientsConfig _instance;
        public static ClientsConfig Instance
        {
            get
            {
                if (_instance == null) _instance = new ClientsConfig();
                return _instance;
            }
        }
    }
}
