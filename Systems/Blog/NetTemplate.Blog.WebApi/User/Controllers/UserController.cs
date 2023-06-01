using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetTemplate.Blog.ApplicationCore.User.Models;
using NetTemplate.Blog.ApplicationCore.User.Queries.GetUsers;
using NetTemplate.Shared.ApplicationCore.Common.Models;
using NetTemplate.Shared.WebApi.Common.Controllers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using Routes = NetTemplate.Blog.WebApi.Common.Constants.ApiRoutes.User;

namespace NetTemplate.Blog.WebApi.User.Controllers
{
    [Route(Routes.Base)]
    public class UserController : ProtectedApiController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserController> _logger;

        public UserController(IMediator mediator,
            ILogger<UserController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet(Routes.GetUsers)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ListResponseModel<UserListItemModel>))]
        public async Task<IActionResult> GetUsers([FromQuery] UserListRequestModel model)
        {
            GetUsersQuery query = new GetUsersQuery(model);

            ListResponseModel<UserListItemModel> response = await _mediator.Send(query);

            return Ok(response);
        }
    }
}
