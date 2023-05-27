namespace NetTemplate.Shared.ApplicationCore.Common.Exceptions
{
    public class InvalidEntityDataException : BaseException
    {
        public InvalidEntityDataException(params string[] properties)
            : base(ResultCodes.Common.InvalidData, data: properties)
        {
        }
    }

}
