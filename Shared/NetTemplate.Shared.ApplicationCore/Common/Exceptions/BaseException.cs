﻿using Microsoft.Extensions.Logging;
using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Shared.ApplicationCore.Common.Exceptions
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

                return new[] { Code.Description };
            }
        }

        public override string Message => string.Join('\n', Messages);

        public object DataObject { get; }
        public ResultCode Code { get; }
        public LogLevel LogLevel { get; }
    }
}
