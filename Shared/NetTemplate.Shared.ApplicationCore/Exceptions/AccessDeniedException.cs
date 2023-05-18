using Fammy.Domain;
using Microsoft.Extensions.Logging;

namespace NetTemplate.Shared.ApplicationCore.Exceptions
{
    public class AccessDeniedException : BaseException
    {
        public AccessDeniedException(ResultCode resultCode = ResultCode.Common_AccessDenied,
            IEnumerable<string> messages = null,
            object data = null,
            LogLevel logLevel = LogLevel.Error) : base(resultCode, messages, data, logLevel)
        {
        }
    }
}
