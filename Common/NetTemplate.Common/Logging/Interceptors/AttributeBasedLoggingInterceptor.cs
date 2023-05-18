using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using NetTemplate.Common.Logging.Attributes;
using NetTemplate.Common.Logging.Extensions;
using NetTemplate.Common.Reflection;

namespace NetTemplate.Common.Logging.Interceptors
{
    public class AttributeBasedLoggingInterceptor : IInterceptor
    {
        private readonly ILogger<AttributeBasedLoggingInterceptor> _logger;
        public AttributeBasedLoggingInterceptor(ILogger<AttributeBasedLoggingInterceptor> logger)
        {
            _logger = logger;
        }

        public void Intercept(IInvocation invocation)
        {
            var method = invocation.Method;
            var logAttribute = method
                .GetCustomAttributes(typeof(LogAttribute), true)
                .FirstOrDefault() as LogAttribute;

            logAttribute = logAttribute ?? method.DeclaringType
                .GetCustomAttributes(typeof(LogAttribute), true)
                .FirstOrDefault() as LogAttribute;

            var shouldLog = logAttribute != null
                && !logAttribute.Disabled
                && (!logAttribute.PublicOnly || method.IsPublic);

            if (shouldLog)
            {
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

            invocation.Proceed();
        }
    }
}
