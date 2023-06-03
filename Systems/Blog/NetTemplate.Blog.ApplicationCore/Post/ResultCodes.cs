using NetTemplate.Shared.ApplicationCore.Common.Models;
using Messages = NetTemplate.Blog.ApplicationCore.Post.Constants.Messages;

namespace NetTemplate.Blog.ApplicationCore.Post
{
    public static class ResultCodes
    {
        public static class Post
        {
            private const int Base = 1500;
            private const string Group = nameof(Post);

            public static readonly ResultCode TitleAlreadyExists = new ResultCode(Base + 1, Messages.TitleAlreadyExists, Group);
        }
    }
}
