using Fammy.Domain;

namespace NetTemplate.Shared.ApplicationCore.Exceptions
{
    public class PersistenceException<T> : TypedDataException<T>
    {
        public PersistenceException(T data) : base(ResultCode.Common_PersistenceError, data: data)
        {
        }
    }

    public class PersistenceException : PersistenceException<object>
    {
        public PersistenceException(object data) : base(data)
        {
        }
    }
}
