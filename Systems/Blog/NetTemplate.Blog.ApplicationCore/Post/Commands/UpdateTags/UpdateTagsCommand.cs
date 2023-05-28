using NetTemplate.Blog.ApplicationCore.Post.Models;
using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Blog.ApplicationCore.Post.Commands.UpdateTags
{
    public class UpdateTagsCommand : ITransactionalCommand
    {
        public int Id { get; }
        public UpdatePostTagsModel Model { get; }

        public UpdateTagsCommand(int id, UpdatePostTagsModel model)
        {
            Id = id;
            Model = model;
        }
    }
}
