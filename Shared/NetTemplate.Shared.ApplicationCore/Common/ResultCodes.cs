using NetTemplate.Shared.ApplicationCore.Common.Models;
using CommonMessages = NetTemplate.Shared.ApplicationCore.Common.Constants.Messages;

namespace NetTemplate.Shared.ApplicationCore.Common
{
    public static class ResultCodes
    {
        public static class Common
        {
            private const int Base = 0;
            private const string Group = nameof(Common);

            public static ResultCode ObjectResult = new ResultCode(Base + 1, CommonMessages.ObjectResult, Group);
            public static ResultCode NotFound = new ResultCode(Base + 2, CommonMessages.NotFound, Group);
            public static ResultCode BadRequest = new ResultCode(Base + 3, CommonMessages.BadRequest, Group);
            public static ResultCode AccessDenied = new ResultCode(Base + 4, CommonMessages.AccessDenied, Group);
            public static ResultCode UnknownError = new ResultCode(Base + 5, CommonMessages.UnknownError, Group);
            public static ResultCode PersistenceError = new ResultCode(Base + 6, CommonMessages.PersistenceError, Group);
            public static ResultCode InvalidPagination = new ResultCode(Base + 7, CommonMessages.InvalidPagination, Group);
            public static ResultCode InvalidData = new ResultCode(Base + 8, CommonMessages.InvalidData, Group);
        }
    }
}
