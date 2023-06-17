using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using NetTemplate.Common.Logging.Extensions;
using NetTemplate.Common.Reflection.Extensions;

namespace NetTemplate.Common.Logging.Interceptors
{
    public class MethodLoggingInterceptor : IInterceptor
    {
        private readonly ILogger<MethodLoggingInterceptor> _logger;
        public MethodLoggingInterceptor(ILogger<MethodLoggingInterceptor> logger)
        {
            _logger = logger;
        }

        public void Intercept(IInvocation invocation)
        {
            var method = invocation.Method;
            var methodDesc = method.GetDescription(invocation.Arguments);
            var targetType = invocation.TargetType.GetGenericTypeName();

            try
            {
                _logger.LogInformation("[START] {targetType} --> {methodDescription}", targetType, methodDesc);
                invocation.Proceed();
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[ERROR] {methodDescription}", methodDesc);
                throw;
            }
        }
    }
}
