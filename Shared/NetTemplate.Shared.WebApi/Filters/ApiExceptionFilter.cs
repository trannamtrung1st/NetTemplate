using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using NetTemplate.Common.Streams;
using NetTemplate.Shared.ApplicationCore.Common.Exceptions;
using NetTemplate.Shared.WebApi.Models;

namespace NetTemplate.Shared.WebApi.Filters
{
    // [IMPORTANT] used to handle predictable exceptions, per action/controller
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<ApiExceptionFilter> _logger;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            LogErrorRequestAsync(context.Exception, context.HttpContext).Wait();
        }

        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            await LogErrorRequestAsync(context.Exception, context.HttpContext);
        }

        private async Task LogErrorRequestAsync(Exception ex, HttpContext httpContext)
        {
            var acceptedLevel = new[] { LogLevel.Error, LogLevel.Critical };
            if (ex is BaseException baseEx && !acceptedLevel.Contains(baseEx.LogLevel))
            {
                return;
            }

            if (ex is ValidationException)
            {
                return;
            }

            var request = httpContext.Request;
            var bodyInfo = string.Empty;

            if (request.ContentLength > 0 &&
                request.ContentLength <= SerilogConfig.Instance.MaxBodyLengthForLogging)
            {
                request.Body.Position = 0;
                var bodyRaw = await request.Body.ReadAsStringAsync();
                bodyInfo = string.IsNullOrEmpty(bodyRaw)
                    ? ""
                    : $"{Environment.NewLine}---- Raw body ----{Environment.NewLine}{bodyRaw}{Environment.NewLine}";
            }

            Dictionary<string, StringValues> form = null;
            if (request.HasFormContentType)
                form = new Dictionary<string, StringValues>(request.Form);

            _logger.LogError("[ERROR] Exception on Request: {@request}{body}", new
            {
                request.ContentLength,
                request.ContentType,
                Host = request.Host.Value,
                request.IsHttps,
                request.Method,
                request.Path,
                request.Protocol,
                QueryString = request.QueryString.Value,
                request.Scheme,
                Form = form
            }, bodyInfo);
        }
    }
}
