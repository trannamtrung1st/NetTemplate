using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using NetTemplate.Shared.WebApi.Authentication.ClientAuthentication;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NetTemplate.Shared.WebApi.Models
{
    public class ConfigureSwaggerGenOptions : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName,
                    CreateVersionInfo(description));
            }

            options.CustomSchemaIds(type => type.FullName.Replace('+', '.'));

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

        private OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
        {
            var apiInfo = new OpenApiInfo()
            {
                Title = WebApiConfig.Instance.ApiTitle,
                Version = description.GroupName,
                Description = WebApiConfig.Instance.ApiDescription
            };

            if (description.IsDeprecated)
            {
                apiInfo.Description += $"{Environment.NewLine}This API version has been deprecated.";
            }

            return apiInfo;
        }
    }

}
