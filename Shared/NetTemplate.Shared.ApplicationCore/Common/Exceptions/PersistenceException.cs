namespace NetTemplate.Shared.ApplicationCore.Common.Exceptions
{
    public class PersistenceException<T> : TypedDataException<T>
    {
        public PersistenceException(T data) : base(ResultCodes.Common.PersistenceError, data: data)
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
