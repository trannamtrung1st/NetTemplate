using NetTemplate.Common.Objects;

namespace NetTemplate.Shared.WebApi.Identity.Models
{
    public class ClientsConfig : ICopyable<ClientsConfig>
    {
        public IEnumerable<ApplicationClient> Clients { get; set; }

        public void CopyTo(ClientsConfig other)
        {
            other.Clients = Clients;
        }
    }
}
