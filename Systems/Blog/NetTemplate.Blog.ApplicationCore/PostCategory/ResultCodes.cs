using NetTemplate.Shared.ApplicationCore.Common.Models;
using Messages = NetTemplate.Blog.ApplicationCore.PostCategory.Constants.Messages;

namespace NetTemplate.Blog.ApplicationCore.PostCategory
{
    public static class ResultCodes
    {
        public static class PostCategory
        {
            private const int Base = 1000;
            private const string Group = nameof(PostCategory);

            public static readonly ResultCode NameAlreadyExists = new ResultCode(Base + 1, Messages.NameAlreadyExists, Group);
        }
    }
}
