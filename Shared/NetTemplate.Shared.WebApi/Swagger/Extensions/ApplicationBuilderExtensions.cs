using Microsoft.AspNetCore.Mvc.ApiExplorer;
using SwaggerConstants = NetTemplate.Shared.WebApi.Swagger.Constants;

namespace NetTemplate.Shared.WebApi.Swagger.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseApplicationSwagger(this IApplicationBuilder app,
            IApiVersionDescriptionProvider apiVersionProvider)
        {
            return app
                .UseSwagger()
                .UseSwaggerUI(options =>
                {
                    foreach (var description in apiVersionProvider.ApiVersionDescriptions)
                    {
                        var versionStr = description.GroupName;
                        options.SwaggerEndpoint(
                            string.Format(SwaggerConstants.DocEndpointFormat, versionStr),
                            versionStr);
                    }

                    options.RoutePrefix = SwaggerConstants.Prefix;
                });
        }
    }
}
