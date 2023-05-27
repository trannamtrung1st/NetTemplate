using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetTemplate.Blog.ApplicationCore.PostCategory.Models;
using NetTemplate.Blog.ApplicationCore.PostCategory.Queries.GetPostCategories;
using NetTemplate.Shared.ApplicationCore.Common.Models;
using NetTemplate.Shared.WebApi.Common.Controllers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using Routes = NetTemplate.Blog.WebApi.Common.Constants.ApiRoutes.PostCategory;

namespace NetTemplate.Blog.WebApi.PostCategory.Controllers
{
    [Route(Routes.Base)]
    public class PostCategoryController : ProtectedApiController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PostCategoryController> _logger;

        public PostCategoryController(IMediator mediator,
            ILogger<PostCategoryController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet(Routes.GetPostCategories)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ListResponseModel<PostCategoryListItemModel>))]
        public async Task<IActionResult> GetPostCategories([FromQuery] PostCategoryListRequestModel model)
        {
            GetPostCategoriesQuery query = new GetPostCategoriesQuery(model);

            ListResponseModel<PostCategoryListItemModel> response = await _mediator.Send(query);

            return Ok(response);
        }
    }
}
