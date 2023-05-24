using Microsoft.Extensions.Logging;
using NetTemplate.Shared.ApplicationCore.Common.Constants;

namespace NetTemplate.Shared.ApplicationCore.Common.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(object data = null, IEnumerable<string> messages = null)
            : base(ResultCode.Common_NotFound, messages, data, LogLevel.Information)
        {
        }
    }
}
