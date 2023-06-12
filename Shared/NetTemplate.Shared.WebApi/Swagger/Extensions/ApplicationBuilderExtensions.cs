using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace NetTemplate.Shared.WebApi.Swagger.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseApplicationSwagger(this IApplicationBuilder app,
            IApiVersionDescriptionProvider apiVersionProvider,
            string docEndpointFormat = DefaultDocEndpointFormat,
            string routePrefix = DefaultRoutePrefix)
        {
            return app
                .UseSwagger()
                .UseSwaggerUI(options =>
                {
                    foreach (var description in apiVersionProvider.ApiVersionDescriptions)
                    {
                        var versionStr = description.GroupName;
                        options.SwaggerEndpoint(
                            string.Format(docEndpointFormat, versionStr),
                            versionStr);
                    }

                    options.RoutePrefix = routePrefix;
                });
        }


        public const string DefaultRoutePrefix = "swagger";
        public const string DefaultDocEndpointFormat = "/swagger/{0}/swagger.json";
    }
}
