using Microsoft.AspNetCore.Mvc;
using NetTemplate.Shared.WebApi.Common.Controllers;
using Messages = NetTemplate.Blog.WebApi.Common.Constants.Messages;
using Routes = NetTemplate.Blog.WebApi.Common.Constants.ApiRoutes.Home;

namespace NetTemplate.Blog.WebApi.Common.Controllers
{
    [Route(Routes.Base)]
    public class HomeController : BaseHomeController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger) : base(logger)
        {
            _logger = logger;
        }

        [HttpGet(Routes.Welcome)]
        public string Welcome(ApiVersion version, [FromServices] IWebHostEnvironment env)
            => GetWelcomeMessage(version, env, Messages.ApiWelcome);
    }
}
