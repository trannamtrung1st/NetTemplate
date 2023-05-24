using FluentValidation;
using Hangfire;
using Hangfire.SqlServer;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetTemplate.Common.DependencyInjection;
using NetTemplate.Common.Logging.Interceptors;
using NetTemplate.Shared.ClientSDK.Common.Extensions;
using NetTemplate.Shared.ClientSDK.Common.Models;
using NetTemplate.Shared.Infrastructure.Background.Filters;
using NetTemplate.Shared.Infrastructure.Background.Models;
using NetTemplate.Shared.Infrastructure.Background.Utils;
using NetTemplate.Shared.Infrastructure.Caching.Implementations;
using NetTemplate.Shared.Infrastructure.Caching.Interfaces;
using NetTemplate.Shared.Infrastructure.Persistence.Models;
using NetTemplate.Shared.Infrastructure.Resilience.Utils;
using NetTemplate.Shared.WebApi.Authentication.ClientAuthentication;
using NetTemplate.Shared.WebApi.Authorization.Implementations;
using NetTemplate.Shared.WebApi.Constants;
using NetTemplate.Shared.WebApi.Filters;
using NetTemplate.Shared.WebApi.Models;
using Polly.Registry;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;

namespace NetTemplate.Shared.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCaching(this IServiceCollection services)
        {
            return services.AddMemoryCache()
                .AddEasyCaching(options =>
                {
                    options.UseInMemory();
                })
                .AddSingleton<ApplicationCache>()
                .AddSingleton<IApplicationCache>(provider => provider.GetRequiredService<ApplicationCache>());
        }

        public static IServiceCollection AddLoggingInterceptors(this IServiceCollection services)
        {
            return services.AddScoped<MethodLoggingInterceptor>()
                .AddScoped<AttributeBasedLoggingInterceptor>();
        }

        public static IServiceCollection AddHangfireDefaults(this IServiceCollection services,
            IConfiguration configuration)
        {
            const int DefaultRetryAttempts = 3;
            const int DefaultSecondsFactor = 2;

            string connStr = configuration.GetConnectionString(nameof(Hangfire));
            string masterConnStr = configuration.GetConnectionString(SharedApiConstants.ConnectionStrings.Master);

            HangfireHelper.InitHangfireDatabase(masterConnStr, HangfireConfig.Instance.DatabaseName).Wait();

            services.AddHangfire(cfg =>
            {
                cfg.UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseFilter(new AutomaticRetryAttribute()
                    {
                        Attempts = DefaultRetryAttempts,
                        DelayInSecondsByAttemptFunc = (attempt) => (int)(DefaultSecondsFactor * attempt)
                    })
                    .UseFilter(new JobLoggingFilter())
                    .UseSerilogLogProvider()

                    // [OPTIONAL] In-memory
                    //.UseInMemoryStorage()

                    // [IMPORTANT] SQL Server integration
                    .UseSqlServerStorage(connStr, new SqlServerStorageOptions
                    {
                        PrepareSchemaIfNecessary = true,
                        UseRecommendedIsolationLevel = true,
                        DisableGlobalLocks = true
                    })
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170);
            });

            services.AddHangfireServer(opt =>
            {
                opt.ServerName = HangfireConfig.Instance.ServerName;

                // [IMPORTANT] for job graceful shutdown
                opt.CancellationCheckInterval = TimeSpan.FromSeconds(5);
            });

            return services;
        }

        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // [TODO]

            return services;
        }

        public static IServiceCollection AddDbContextDefaults<T>(this IServiceCollection services, IConfiguration configuration,
            string connStr) where T : DbContext
        {
            if (connStr is null) throw new ArgumentNullException(nameof(connStr));

            services.AddDbContext<T>((provider, options) =>
            {
                options
                    .UseSqlServer(connStr)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);

                if (DatabaseConfig.Instance.EnableDebug)
                {
                    var loggerFactory = LoggerFactory.Create(factory =>
                        factory.AddConsole());

                    options
                        .EnableSensitiveDataLogging()
                        .EnableDetailedErrors()
                        .UseLoggerFactory(loggerFactory);
                }
            });

            return services;
        }

        public static IServiceCollection AddMediator(this IServiceCollection services,
            Assembly[] assemblies)
        {
            services.Scan(scan => scan.FromApplicationDependencies(
                    assembly => assembly.GetName()?.Name?.StartsWith(nameof(NetTemplate)) == true)
                .AddClasses(classes => classes.AssignableTo(typeof(IPipelineBehavior<,>)))
                .As(typeof(IPipelineBehavior<,>))
                .WithScopedLifetime());

            return services.AddMediatR(config =>
            {
                config.Lifetime = ServiceLifetime.Scoped;
                config.RegisterServicesFromAssemblies(assemblies);
            });
        }

        public static IServiceCollection AddClientSdkServices(this IServiceCollection services)
        {
            services.AddClientSdkHandlers()
                .AddTokenManagement(new ClientConfiguration
                {
                    ClientId = IdentityConfig.Instance.ClientId,
                    ClientSecret = IdentityConfig.Instance.ClientSecret,
                    IdentityServerUrl = IdentityConfig.Instance.ServerUrl
                });

            return services;
        }

        public static IServiceCollection AddResilience(this IServiceCollection services)
        {
            return services.AddSingleton<IReadOnlyPolicyRegistry<string>, PolicyRegistry>(
                (serviceProvider) => PollyHelper.InitPolly(serviceProvider));
        }

        public static IServiceCollection AddApiVersioningDefaults(this IServiceCollection services)
        {
            return services.AddApiVersioning(opt =>
            {
                opt.ApiVersionReader = new UrlSegmentApiVersionReader();
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
            })
                .AddVersionedApiExplorer(opt =>
                {
                    opt.GroupNameFormat = SharedApiConstants.Versioning.GroupNameFormat;
                    //opt.SubstituteApiVersionInUrl = true;
                });
        }

        public static IServiceCollection AddAuthenticationDefaults(this IServiceCollection services,
            IWebHostEnvironment environment)
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
                        ValidIssuer = JwtConfig.Instance.Issuer,
                        ValidAudience = JwtConfig.Instance.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfig.Instance.Secret))
                    };
                })
                // Client authentication
                .AddScheme<ClientAuthenticationOptions, ClientAuthenticationHandler>(
                    ClientAuthenticationDefaults.AuthenticationScheme, opt =>
                    {
                        opt.Clients = ClientsConfig.Instance?.Clients;
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

        public static IServiceCollection AddPubSubIntegration(this IServiceCollection services, IConfiguration configuration)
        {
            // [TODO]

            return services;
        }

        public static IServiceCollection AddMapper(this IServiceCollection services,
            Assembly[] assemblies)
        {
            return services.AddAutoMapper(assemblies);
        }

        public static IServiceCollection AddSwaggerDefaults(this IServiceCollection services)
        {
            services.AddSwaggerGenNewtonsoftSupport();
            services.AddSwaggerGen();
            services.ConfigureOptions<ConfigureSwaggerGenOptions>();
            return services;
        }

        public static IServiceCollection ConfigureApiBehavior(this IServiceCollection services)
        {
            return services.Configure<ApiBehaviorOptions>(opt =>
            {
                // [IMPORTANT] Disable automatic 400 response
                opt.SuppressModelStateInvalidFilter = true;
            });
        }

        public static IMvcBuilder AddControllersDefaults(this IServiceCollection services,
            Action<MvcOptions> extraAction)
        {
            return services.AddControllers(opt =>
            {
                opt.Filters.Add<ValidateModelStateFilter>();
                opt.Filters.Add<ApiExceptionFilter>();
                opt.Filters.Add<ApiResponseWrapperFilter>();

                extraAction(opt);
            });
        }

        public static IServiceCollection AddValidationDefaults(this IServiceCollection services, Assembly[] assemblies)
        {
            ValidatorOptions.Global.DisplayNameResolver = (type, member, expression) =>
                ValidatorOptions.Global.PropertyNameResolver(type, member, expression);

            return services
                .AddValidatorsFromAssemblies(assemblies);
        }

        public static IServiceCollection ScanServices(this IServiceCollection services,
            IEnumerable<Assembly> assemblies)
        {
            return services.Scan(scan => scan.FromAssemblies(assemblies)
                .AddClasses(classes => classes.WithAttribute<TransientServiceAttribute>())
                .AsSelfWithInterfaces()
                .WithTransientLifetime()

                .AddClasses(classes => classes.WithAttribute<ScopedServiceAttribute>())
                .AsSelfWithInterfaces()
                .WithScopedLifetime()

                .AddClasses(classes => classes.WithAttribute<SingletonServiceAttribute>())
                .AsSelfWithInterfaces()
                .WithSingletonLifetime()

                .AddClasses(classes => classes.WithAttribute<SelfTransientServiceAttribute>())
                .AsSelf()
                .WithTransientLifetime()

                .AddClasses(classes => classes.WithAttribute<SelfScopedServiceAttribute>())
                .AsSelf()
                .WithScopedLifetime()

                .AddClasses(classes => classes.WithAttribute<SelfSingletonServiceAttribute>())
                .AsSelf()
                .WithSingletonLifetime());
        }
    }
}
