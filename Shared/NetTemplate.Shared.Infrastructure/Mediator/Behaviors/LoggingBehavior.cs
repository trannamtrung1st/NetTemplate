using MediatR;
using Microsoft.Extensions.Logging;
using NetTemplate.Common.Reflection;

namespace NetTemplate.Shared.Infrastructure.Mediator.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) => _logger = logger;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestTypeName = request.GetGenericTypeName();
            _logger.LogInformation("[START] Handling command {commandName}", requestTypeName);
            var response = await next();
            _logger.LogInformation("[END] Command {commandName} handled", requestTypeName);

            return response;
        }
    }
}
