using Microsoft.Extensions.Logging;
using CommonMessages = NetTemplate.Shared.ApplicationCore.Common.Constants.Messages;

namespace NetTemplate.Shared.ApplicationCore.Common.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string entityName = null, object data = null, IEnumerable<string> messages = null)
            : base(ResultCodes.Common.NotFound,
                  messages ?? (entityName != null
                    ? new[] { CommonMessages.EntityNotFound.Replace("{EntityName}", entityName) }
                    : null),
                  data, LogLevel.Information)
        {
        }
    }
}
