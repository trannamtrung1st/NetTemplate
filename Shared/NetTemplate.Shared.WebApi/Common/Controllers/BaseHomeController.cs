using Microsoft.AspNetCore.Mvc;
using System.Text;
using CommonMessages = NetTemplate.Shared.WebApi.Common.Constants.Messages;

namespace NetTemplate.Shared.WebApi.Common.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class BaseHomeController : BaseApiController
    {
        private readonly ILogger<BaseHomeController> _logger;

        public BaseHomeController(ILogger<BaseHomeController> logger)
        {
            _logger = logger;
        }

        protected virtual string GetWelcomeMessage(
            ApiVersion version, IWebHostEnvironment env, string apiWelcomeMessageFormat)
        {
            StringBuilder finalMessage = new StringBuilder()
                .AppendFormat(apiWelcomeMessageFormat, env.EnvironmentName, version);

            if (!env.IsProduction())
            {
                finalMessage
                    .AppendLine()
                    .Append(CommonMessages.SwaggerInstruction);
            }

            return finalMessage.ToString();
        }
    }
}
