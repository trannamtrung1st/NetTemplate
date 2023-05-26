using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace NetTemplate.Shared.WebApi.Common.Controllers
{
    [Authorize]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    [SwaggerResponse((int)HttpStatusCode.Forbidden)]
    public abstract class ProtectedApiController : BaseApiController
    {

    }
}
