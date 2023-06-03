using Microsoft.Extensions.Logging;
using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Shared.ApplicationCore.Common.Exceptions
{
    public class BusinessException : BaseException
    {
        public BusinessException(
            ResultCode resultCode,
            IEnumerable<string> messages = null,
            object data = null,
            LogLevel logLevel = LogLevel.Information) : base(resultCode, messages, data, logLevel)
        {
        }
    }
}
