using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using NetTemplate.Common.DependencyInjection;
using NetTemplate.Shared.ApplicationCore.Identity.Interfaces;
using NetTemplate.Shared.WebApi.Identity.Implementations;
using NetTemplate.Shared.WebApi.Identity.Models;
using NetTemplate.Shared.WebApi.Identity.Schemes.ClientAuthentication;
using NetTemplate.Shared.WebApi.Identity.Schemes.SimulatedAuthentication;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace NetTemplate.Shared.WebApi.Identity.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRequestCurrentUserProvider(this IServiceCollection services)
            => services.AddScoped<ICurrentUserProvider, RequestCurrentUserProvider>();

        public static IServiceCollection AddAuthenticationDefaults(this IServiceCollection services,
            JwtConfig jwtConfig, ClientsConfig clientsConfig, IWebHostEnvironment environment,
            SimulatedAuthConfig simulatedAuthConfig = null)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            AuthenticationBuilder authBuilder = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                // Client authentication
                .AddScheme<ClientAuthenticationOptions, ClientAuthenticationHandler>(
                    ClientAuthenticationDefaults.AuthenticationScheme, opt =>
                    {
                        opt.Clients = clientsConfig?.Clients;
                    });

            if (simulatedAuthConfig?.Enabled == true)
            {
                authBuilder = authBuilder
                    .AddScheme<SimulatedAuthenticationOptions, SimulatedAuthenticationHandler>(
                        SimulatedAuthenticationDefaults.AuthenticationScheme, opt =>
                        {
                            opt.UserId = simulatedAuthConfig.UserId;
                            opt.UserCode = simulatedAuthConfig.UserCode;
                            opt.Claims = simulatedAuthConfig.Claims;
                        });

                services.ConfigureCopyableConfig(simulatedAuthConfig);
            }
            else
            {
                authBuilder = authBuilder
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
                    });
            }

            services.ConfigureCopyableConfig(jwtConfig)
                .ConfigureCopyableConfig(clientsConfig);

            return services;
        }

        public static IServiceCollection AddAuthorizationDefaults(this IServiceCollection services)
        {
            return services.AddAuthorization(opt =>
            {
                // [TODO] authorize actions
            }).AddSingleton<IAuthorizationPolicyProvider, ApplicationPolicyProvider>();
        }
    }
}
