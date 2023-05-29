using NetTemplate.Blog.ApplicationCore.Post.Models;
using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Blog.ApplicationCore.Post.Commands.UpdatePostTags
{
    public class UpdatePostTagsCommand : ITransactionalCommand
    {
        public int Id { get; }
        public UpdatePostTagsModel Model { get; }

        public UpdatePostTagsCommand(int id, UpdatePostTagsModel model)
        {
            Id = id;
            Model = model;
        }
    }
}
