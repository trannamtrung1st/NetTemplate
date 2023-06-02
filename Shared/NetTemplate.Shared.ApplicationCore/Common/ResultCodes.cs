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

            public static readonly ResultCode ObjectResult = new ResultCode(Base + 1, CommonMessages.ObjectResult, Group);
            public static readonly ResultCode NotFound = new ResultCode(Base + 2, CommonMessages.NotFound, Group);
            public static readonly ResultCode BadRequest = new ResultCode(Base + 3, CommonMessages.BadRequest, Group);
            public static readonly ResultCode AccessDenied = new ResultCode(Base + 4, CommonMessages.AccessDenied, Group);
            public static readonly ResultCode UnknownError = new ResultCode(Base + 5, CommonMessages.UnknownError, Group);
            public static readonly ResultCode PersistenceError = new ResultCode(Base + 6, CommonMessages.PersistenceError, Group);
            public static readonly ResultCode InvalidPagination = new ResultCode(Base + 7, CommonMessages.InvalidPagination, Group);
            public static readonly ResultCode InvalidData = new ResultCode(Base + 8, CommonMessages.InvalidData, Group);
        }
    }
}
