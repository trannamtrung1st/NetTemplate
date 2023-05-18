﻿using Fammy.Domain;
using Microsoft.Extensions.Logging;

namespace NetTemplate.Shared.ApplicationCore.Exceptions
{
    public abstract class TypedDataException<T> : BaseException
    {
        public T TypedDataObject => (T)DataObject;

        protected TypedDataException(
            ResultCode resultCode,
            IEnumerable<string> messages = null,
            T data = default,
            LogLevel logLevel = LogLevel.Error) : base(resultCode, messages, data, logLevel)
        {
        }
    }
}
