using NetTemplate.Blog.ApplicationCore.Post.Models;
using NetTemplate.Shared.ApplicationCore.Models;

namespace NetTemplate.Blog.ApplicationCore.Post.Commands.UpdateTags
{
    public class UpdateTagsCommand : ITransactionalCommand
    {
        public UpdatePostTagsModel Model { get; }

        public UpdateTagsCommand(UpdatePostTagsModel model)
        {
            Model = model;
        }
    }
}
