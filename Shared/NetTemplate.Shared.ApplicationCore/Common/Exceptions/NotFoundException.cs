using Microsoft.Extensions.Logging;

namespace NetTemplate.Shared.ApplicationCore.Common.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(object data = null, IEnumerable<string> messages = null)
            : base(ResultCodes.Common.NotFound, messages, data, LogLevel.Information)
        {
        }
    }
}
