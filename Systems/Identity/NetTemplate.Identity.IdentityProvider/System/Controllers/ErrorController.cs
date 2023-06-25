using Microsoft.AspNetCore.Mvc;
using NetTemplate.Shared.WebApi.Common.Controllers;
using Routes = NetTemplate.Identity.IdentityProvider.Common.Constants.ApiRoutes.Error;

namespace NetTemplate.Identity.IdentityProvider.System.Controllers
{
    [Route(Routes.Base)]
    public class ErrorController : BaseErrorController
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(IWebHostEnvironment env,
            ILogger<ErrorController> logger) : base(env, logger)
        {
            _logger = logger;
        }

        [Route(Routes.HandleException)]
        public IActionResult HandleException() => HandleCommonException();
    }
}
