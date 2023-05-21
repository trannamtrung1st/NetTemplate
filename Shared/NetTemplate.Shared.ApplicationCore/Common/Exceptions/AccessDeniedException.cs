using Microsoft.Extensions.Logging;
using NetTemplate.Shared.ApplicationCore.Common.Constants;

namespace NetTemplate.Shared.ApplicationCore.Common.Exceptions
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
