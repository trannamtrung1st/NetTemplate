using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetTemplate.Blog.ApplicationCore.Comment.Commands.DeleteComment;
using NetTemplate.Blog.ApplicationCore.Comment.Commands.UpdateComment;
using NetTemplate.Blog.ApplicationCore.Comment.Models;
using NetTemplate.Blog.ApplicationCore.Comment.Queries.GetCommentDetails;
using NetTemplate.Shared.WebApi.Common.Controllers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using Routes = NetTemplate.Blog.WebApi.Common.Constants.ApiRoutes.Comment;

namespace NetTemplate.Blog.WebApi.Comment.Controllers
{
    [Route(Routes.Base)]
    public class CommentController : ProtectedApiController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CommentController> _logger;

        public CommentController(IMediator mediator,
            ILogger<CommentController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet(Routes.GetCommentDetails)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(CommentDetailsModel))]
        public async Task<IActionResult> GetCommentDetails([FromRoute] int id)
        {
            GetCommentDetailsQuery query = new GetCommentDetailsQuery(id);

            CommentDetailsModel response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpPut(Routes.UpdateComment)]
        [SwaggerResponse((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> UpdateComment(
            [FromRoute] int id, [FromBody] UpdateCommentModel model)
        {
            UpdateCommentCommand command = new UpdateCommentCommand(id, model);

            await _mediator.Send(command);

            return NoContent();
        }

        [HttpDelete(Routes.DeleteComment)]
        [SwaggerResponse((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            DeleteCommentCommand command = new DeleteCommentCommand(id);

            await _mediator.Send(command);

            return NoContent();
        }
    }
}
