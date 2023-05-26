using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NetTemplate.Shared.ApplicationCore.Common.Exceptions;
using NetTemplate.Shared.WebApi.Common.Models;
using System.Net;

namespace NetTemplate.Shared.WebApi.Common.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public abstract class BaseErrorController : BaseApiController
    {
        private readonly ILogger<BaseErrorController> _logger;

        protected readonly IWebHostEnvironment env;

        public BaseErrorController(IWebHostEnvironment env,
            ILogger<BaseErrorController> logger)
        {
            this.env = env;
            _logger = logger;
        }

        protected virtual IActionResult HandleCommonException()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context.Error;

            if (exception == null) return BadRequest();

            ApiResponse response = null;

            if (exception is NotFoundException notFoundEx)
            {
                response = ApiResponse.Exception(notFoundEx);
                return NotFound(response);
            }
            else if (exception is AccessDeniedException accessDeniedEx)
            {
                response = ApiResponse.Exception(accessDeniedEx);
                return AccessDenied(response);
            }
            else if (exception is ValidationException validationEx)
            {
                response = ApiResponse.BadRequest(validationEx);
                return BadRequest(response);
            }
            else if (exception is BaseException baseEx)
            {
                response = ApiResponse.Exception(baseEx);
                return BadRequest(response);
            }

            if (response == null)
            {
                if (env.IsDevelopment())
                    response = ApiResponse.UnknownError(exception, new[] { exception.Message });
                else response = ApiResponse.UnknownError();
            }

            _logger.LogError("[ERROR] {statusCode} {statusText}",
                (int)HttpStatusCode.InternalServerError,
                nameof(HttpStatusCode.InternalServerError));

            return Error(response);
        }
    }
}
