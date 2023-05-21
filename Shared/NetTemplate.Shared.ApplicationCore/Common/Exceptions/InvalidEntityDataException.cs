using NetTemplate.Shared.ApplicationCore.Common.Constants;

namespace NetTemplate.Shared.ApplicationCore.Common.Exceptions
{
    public class InvalidEntityDataException : BaseException
    {
        public InvalidEntityDataException(params string[] properties)
            : base(ResultCode.Common_InvalidData, data: properties)
        {
        }
    }

}
