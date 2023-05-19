using Microsoft.Extensions.Logging;
using NetTemplate.Shared.ApplicationCore.Constants;

namespace NetTemplate.Shared.ApplicationCore.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(object data = null, IEnumerable<string> messages = null)
            : base(ResultCode.Common_PostNotFound, messages, data, LogLevel.Information)
        {
        }
    }
}
