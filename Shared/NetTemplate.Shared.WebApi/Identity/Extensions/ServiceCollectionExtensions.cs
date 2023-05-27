using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using NetTemplate.Shared.WebApi.Identity.Implementations;
using NetTemplate.Shared.WebApi.Identity.Models;
using NetTemplate.Shared.WebApi.Identity.Schemes.ClientAuthentication;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace NetTemplate.Shared.WebApi.Identity.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthenticationDefaults(this IServiceCollection services,
            JwtConfig jwtConfig, ClientsConfig clientsConfig, IWebHostEnvironment environment)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                // JWT tokens
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtConfig.Issuer,
                        ValidAudience = jwtConfig.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret))
                    };
                })
                // Client authentication
                .AddScheme<ClientAuthenticationOptions, ClientAuthenticationHandler>(
                    ClientAuthenticationDefaults.AuthenticationScheme, opt =>
                    {
                        opt.Clients = clientsConfig?.Clients;
                    });

            return services;
        }

        public static IServiceCollection AddAuthorizationDefaults(this IServiceCollection services)
        {
            return services.AddAuthorization(opt =>
            {
                // [TODO]
            }).AddSingleton<IAuthorizationPolicyProvider, ApplicationPolicyProvider>();
        }
    }
}
