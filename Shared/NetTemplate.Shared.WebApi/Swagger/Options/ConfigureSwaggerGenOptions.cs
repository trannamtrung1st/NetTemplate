using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using NetTemplate.Common.Reflection;
using NetTemplate.Shared.WebApi.Common.Models;
using NetTemplate.Shared.WebApi.Identity.Schemes.ClientAuthentication;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NetTemplate.Shared.WebApi.Swagger.Options
{
    public class ConfigureSwaggerGenOptions : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;
        private readonly IOptions<WebInfoConfig> _webInfoOptions;

        public ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider provider,
            IOptions<WebInfoConfig> webInfoOptions)
        {
            _provider = provider;
            _webInfoOptions = webInfoOptions;
        }

        public void Configure(SwaggerGenOptions options)
        {
            WebInfoConfig webInfoConfig = _webInfoOptions.Value;

            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName,
                    CreateVersionInfo(description, webInfoConfig));
            }

            options.CustomSchemaIds(type => type.GetGenericTypeName().Replace('+', '.'));

            options.EnableAnnotations();

            const string ApplicationApiKey = nameof(ApplicationApiKey);
            const string ApplicationClient = nameof(ApplicationClient);

            options.AddSecurityDefinition(ApplicationApiKey,
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter an Authorization Header value",
                    Name = HeaderNames.Authorization,
                    Type = SecuritySchemeType.ApiKey,
                });

            options.AddSecurityDefinition(ApplicationClient,
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a credentials",
                    Name = HeaderNames.Authorization,
                    Type = SecuritySchemeType.Http,
                    Scheme = ClientAuthenticationDefaults.AuthenticationScheme
                });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = ApplicationApiKey
                        }
                    },
                    new string[0]
                },
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = ApplicationClient
                        }
                    },
                    new string[0]
                }
            });
        }

        public void Configure(string name, SwaggerGenOptions options)
        {
            Configure(options);
        }

        private OpenApiInfo CreateVersionInfo(ApiVersionDescription description,
            WebInfoConfig webInfoConfig)
        {
            var apiInfo = new OpenApiInfo()
            {
                Title = webInfoConfig.ApiTitle,
                Description = webInfoConfig.ApiDescription,
                Version = description.GroupName
            };

            if (description.IsDeprecated)
            {
                apiInfo.Description += $"{Environment.NewLine}This API version has been deprecated.";
            }

            return apiInfo;
        }
    }

}
