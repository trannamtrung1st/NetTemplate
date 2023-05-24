using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NetTemplate.Shared.WebApi.Models;

namespace NetTemplate.Shared.WebApi.Filters
{
    public class ValidateModelStateFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
            {
                return;
            }

            var apiResponse = ApiResponse.BadRequest(context.ModelState);

            context.Result = new BadRequestObjectResult(apiResponse);
        }
    }
}
