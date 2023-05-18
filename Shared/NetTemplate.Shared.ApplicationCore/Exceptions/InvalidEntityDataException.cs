using Fammy.Domain;

namespace NetTemplate.Shared.ApplicationCore.Exceptions
{
    public class InvalidEntityDataException : BaseException
    {
        public InvalidEntityDataException(params string[] properties)
            : base(ResultCode.Common_InvalidData, data: properties)
        {
        }
    }

}
