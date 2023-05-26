using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using NetTemplate.Common.Streams;
using NetTemplate.Shared.ApplicationCore.Common.Exceptions;

namespace NetTemplate.Shared.WebApi.Common.Filters
{
    // [IMPORTANT] used to handle predictable exceptions, per action/controller
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<ApiExceptionFilter> _logger;
        private readonly IConfiguration _configuration;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
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
            var maxBodyLengthForLogging = _configuration.GetValue<long>("LoggingConfig:MaxBodyLengthForLogging");

            if (request.ContentLength > 0 && request.ContentLength <= maxBodyLengthForLogging)
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
