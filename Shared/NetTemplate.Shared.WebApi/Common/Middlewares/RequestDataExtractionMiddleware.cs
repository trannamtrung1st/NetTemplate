using NetTemplate.Shared.ApplicationCore.Identity.Interfaces;
using NetTemplate.Shared.WebApi.Common.Constants;
using Serilog.Context;

namespace NetTemplate.Shared.WebApi.Common.Middlewares
{
    public class RequestDataExtractionMiddleware : IMiddleware
    {
        private readonly ICurrentUserProvider _currentUserProvider;

        public RequestDataExtractionMiddleware(ICurrentUserProvider currentUserProvider)
        {
            _currentUserProvider = currentUserProvider;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (_currentUserProvider.UserCode != null)
            {
                LogContext.PushProperty(SharedApiConstants.LogProperties.UserCode, _currentUserProvider.UserCode);
            }

            await next(context);
        }
    }
}
