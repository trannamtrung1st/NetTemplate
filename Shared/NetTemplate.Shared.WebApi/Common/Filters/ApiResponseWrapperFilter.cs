using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using NetTemplate.Common.Reflection;
using NetTemplate.Shared.WebApi.Common.Attributes;
using NetTemplate.Shared.WebApi.Common.Models;

namespace NetTemplate.Shared.WebApi.Common.Filters
{
    public class ApiResponseWrapperFilter : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var controllerAction = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerAction == null)
            {
                return;
            }

            var objectResult = context.Result as ObjectResult;
            if (objectResult == null)
            {
                return;
            }

            if (!typeof(Task<IActionResult>).IsAssignableFrom(controllerAction.MethodInfo.ReturnType)
                && !typeof(IActionResult).IsAssignableFrom(controllerAction.MethodInfo.ReturnType))
            {
                return;
            }

            var disableWrap = ReflectionHelper.GetAttributesOfMemberOrType<NoWrapResponse>(
                controllerAction.MethodInfo).Any();

            if (disableWrap)
            {
                return;
            }

            if (objectResult.Value is ApiResponse == false)
            {
                objectResult.Value = ApiResponse.Object(objectResult.Value);
            }
        }
    }
}
