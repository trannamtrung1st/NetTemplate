using NetTemplate.Shared.ApplicationCore.Common.Models;
using Messages = NetTemplate.Blog.ApplicationCore.User.Constants.Messages;

namespace NetTemplate.Blog.ApplicationCore.User
{
    public static class ResultCodes
    {
        public static class User
        {
            private const int Base = 500;
            private const string Group = nameof(User);

            public static readonly ResultCode UserAlreadyExists = new ResultCode(Base + 1, Messages.UserAlreadyExists, Group);
        }
    }
}
