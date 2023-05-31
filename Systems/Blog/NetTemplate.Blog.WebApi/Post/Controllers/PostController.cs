using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetTemplate.Blog.ApplicationCore.Comment.Commands.CreatePostComment;
using NetTemplate.Blog.ApplicationCore.Comment.Models;
using NetTemplate.Blog.ApplicationCore.Comment.Queries.GetPostComments;
using NetTemplate.Blog.ApplicationCore.Post.Commands.CreatePost;
using NetTemplate.Blog.ApplicationCore.Post.Commands.DeletePost;
using NetTemplate.Blog.ApplicationCore.Post.Commands.UpdatePost;
using NetTemplate.Blog.ApplicationCore.Post.Commands.UpdatePostTags;
using NetTemplate.Blog.ApplicationCore.Post.Models;
using NetTemplate.Blog.ApplicationCore.Post.Queries.GetPostDetails;
using NetTemplate.Blog.ApplicationCore.Post.Queries.GetPosts;
using NetTemplate.Shared.ApplicationCore.Common.Models;
using NetTemplate.Shared.WebApi.Common.Controllers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using Routes = NetTemplate.Blog.WebApi.Common.Constants.ApiRoutes.Post;

namespace NetTemplate.Blog.WebApi.Post.Controllers
{
    [Route(Routes.Base)]
    public class PostController : ProtectedApiController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PostController> _logger;

        public PostController(IMediator mediator,
            ILogger<PostController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet(Routes.GetPosts)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ListResponseModel<PostListItemModel>))]
        public async Task<IActionResult> GetPostCategories([FromQuery] PostListRequestModel model)
        {
            GetPostsQuery query = new GetPostsQuery(model);

            ListResponseModel<PostListItemModel> response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpGet(Routes.GetPostDetails)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(PostDetailsModel))]
        public async Task<IActionResult> GetPostDetails([FromRoute] int id)
        {
            GetPostDetailsQuery query = new GetPostDetailsQuery(id);

            PostDetailsModel response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpGet(Routes.GetPostComments)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ListResponseModel<CommentListItemModel>))]
        public async Task<IActionResult> GetPostComments([FromRoute] int id, [FromQuery] CommentListRequestModel model)
        {
            GetPostCommentsQuery query = new GetPostCommentsQuery(id, model);

            ListResponseModel<CommentListItemModel> response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpPost(Routes.CreatePost)]
        [SwaggerResponse((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostModel model)
        {
            CreatePostCommand command = new CreatePostCommand(model);

            await _mediator.Send(command);

            return NoContent();
        }

        [HttpPost(Routes.CreatePostComment)]
        [SwaggerResponse((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> CreatePostComment(
            [FromRoute] int id, [FromBody] CreateCommentModel model)
        {
            CreatePostCommentCommand command = new CreatePostCommentCommand(id, model);

            await _mediator.Send(command);

            return NoContent();
        }

        [HttpPut(Routes.UpdatePost)]
        [SwaggerResponse((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> UpdatePost(
            [FromRoute] int id, [FromBody] UpdatePostModel model)
        {
            UpdatePostCommand command = new UpdatePostCommand(id, model);

            await _mediator.Send(command);

            return NoContent();
        }

        [HttpPut(Routes.UpdatePostTags)]
        [SwaggerResponse((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> UpdatePostTags(
            [FromRoute] int id, [FromBody] UpdatePostTagsModel model)
        {
            UpdatePostTagsCommand command = new UpdatePostTagsCommand(id, model);

            await _mediator.Send(command);

            return NoContent();
        }

        [HttpDelete(Routes.DeletePost)]
        [SwaggerResponse((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeletePost([FromRoute] int id)
        {
            DeletePostCommand command = new DeletePostCommand(id);

            await _mediator.Send(command);

            return NoContent();
        }
    }
}
