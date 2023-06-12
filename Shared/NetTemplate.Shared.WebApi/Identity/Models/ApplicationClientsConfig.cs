using NetTemplate.Common.Objects;

namespace NetTemplate.Shared.WebApi.Identity.Models
{
    public class ApplicationClientsConfig : ICopyable<ApplicationClientsConfig>
    {
        public IEnumerable<ApplicationClient> Clients { get; set; }

        public void CopyTo(ApplicationClientsConfig other)
        {
            other.Clients = Clients;
        }


        public const string ConfigurationSection = nameof(ApplicationClientsConfig);
    }
}
