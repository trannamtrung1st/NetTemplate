using MediatR;
using Microsoft.Extensions.Logging;
using NetTemplate.Common.Reflection;

namespace NetTemplate.Shared.Infrastructure.Mediator.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IBaseRequest
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) => _logger = logger;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestTypeName = request.GetGenericTypeName();
            _logger.LogInformation("[START] Handling request {requestName}", requestTypeName);
            var response = await next();
            _logger.LogInformation("[END] Request {requestName} handled", requestTypeName);

            return response;
        }
    }
}
