using Microsoft.AspNetCore.Mvc.ApiExplorer;

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
                            string.Format(SharedApiConstants.SwaggerDefaults.DocEndpointFormat, versionStr),
                            versionStr);
                    }

                    options.RoutePrefix = SharedApiConstants.SwaggerDefaults.Prefix;
                });
        }
    }
}
