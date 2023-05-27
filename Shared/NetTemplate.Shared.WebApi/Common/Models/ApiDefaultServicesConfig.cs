using Microsoft.AspNetCore.Mvc;
using NetTemplate.Shared.Infrastructure.Common.Models;
using NetTemplate.Shared.WebApi.Identity.Models;

namespace NetTemplate.Shared.WebApi.Common.Models
{
    public class ApiDefaultServicesConfig : DefaultServicesConfig
    {
        // Common
        public Action<MvcOptions> ControllerConfigureAction { get; set; }

        // Identity
        public JwtConfig JwtConfig { get; set; }
        public ClientsConfig ClientsConfig { get; set; }
        public SimulatedAuthConfig SimulatedAuthConfig { get; set; }
    }
}
