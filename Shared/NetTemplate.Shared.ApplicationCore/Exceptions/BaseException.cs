using Fammy.Domain;
using Microsoft.Extensions.Logging;
using NetTemplate.Common.Enumerations;

namespace NetTemplate.Shared.ApplicationCore.Exceptions
{
    public abstract class BaseException : Exception
    {
        public BaseException(ResultCode resultCode,
            IEnumerable<string> messages = null,
            object data = null,
            LogLevel logLevel = LogLevel.Error)
        {
            Code = resultCode;
            DataObject = data;
            _messages = messages;
            LogLevel = logLevel;
        }

        private IEnumerable<string> _messages;
        public IEnumerable<string> Messages
        {
            get
            {
                if (_messages != null) return _messages;

                string represent = Code.GetDescription() ?? Code.GetDisplayName() ?? Code.GetName();

                return new[] { represent };
            }
        }

        public override string Message => string.Join('\n', Messages);

        public object DataObject { get; }
        public ResultCode Code { get; }
        public LogLevel LogLevel { get; }
    }
}
