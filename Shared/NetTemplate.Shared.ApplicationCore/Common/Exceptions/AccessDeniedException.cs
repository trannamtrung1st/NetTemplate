using Microsoft.Extensions.Logging;
using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Shared.ApplicationCore.Common.Exceptions
{
    public class AccessDeniedException : BaseException
    {
        public AccessDeniedException(ResultCode? resultCode = default,
            IEnumerable<string> messages = null,
            object data = null,
            LogLevel logLevel = LogLevel.Error)
            : base(resultCode ?? ResultCodes.Common.AccessDenied, messages, data, logLevel)
        {
        }
    }
}
