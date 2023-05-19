using Microsoft.Extensions.Logging;
using NetTemplate.Shared.ApplicationCore.Constants;

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
