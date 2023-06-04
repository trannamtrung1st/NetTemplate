using Autofac;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using NetTemplate.Blog.Infrastructure.Common.Extensions;
using NetTemplate.Blog.Infrastructure.Persistence;
using NetTemplate.Blog.WebApi.Common.Models;
using NetTemplate.Common.Web.Middlewares;
using NetTemplate.Redis.Extensions;
using NetTemplate.Redis.Models;
using NetTemplate.Shared.ApplicationCore.Common.Events;
using NetTemplate.Shared.ClientSDK.Common.Models;
using NetTemplate.Shared.Infrastructure.Background.Extensions;
using NetTemplate.Shared.Infrastructure.Background.Models;
using NetTemplate.Shared.Infrastructure.Common.Extensions;
using NetTemplate.Shared.Infrastructure.Common.Utils;
using NetTemplate.Shared.Infrastructure.Identity.Extensions;
using NetTemplate.Shared.Infrastructure.Identity.Models;
using NetTemplate.Shared.Infrastructure.PubSub.Extensions;
using NetTemplate.Shared.Infrastructure.PubSub.Models;
using NetTemplate.Shared.WebApi.Common.Extensions;
using NetTemplate.Shared.WebApi.Common.Models;
using NetTemplate.Shared.WebApi.Common.Utils;
using NetTemplate.Shared.WebApi.Identity.Extensions;
using NetTemplate.Shared.WebApi.Identity.Models;
using NetTemplate.Shared.WebApi.Logging.Extensions;
using NetTemplate.Shared.WebApi.Swagger.Extensions;
using Serilog.Extensions.Logging;
using System.Dynamic;
using System.Reflection;
using ApiRoutes = NetTemplate.Blog.WebApi.Common.Constants.ApiRoutes;
using BackgroundConnectionNames = NetTemplate.Shared.Infrastructure.Background.Constants.ConnectionNames;
using CacheProfiles = NetTemplate.Blog.WebApi.Common.Constants.CacheProfiles;
using CommonConfigurationSections = NetTemplate.Blog.ApplicationCore.Common.Constants.ConfigurationSections;
using WebLoggingConfigurationSections = NetTemplate.Shared.WebApi.Logging.Constants.ConfigurationSections;

// ===== APPLICATION START =====

bool isProduction = WebApplicationHelper.IsProduction();
using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
using Serilog.Core.Logger seriLogger = InfrastructureHelper.CreateHostLogger(isProduction);
ILogger logger = new SerilogLoggerFactory(seriLogger).CreateLogger(nameof(Program));
List<IDisposable> resources = new List<IDisposable>();
CancellationToken cancellationToken = cancellationTokenSource.Token;

try
{
    logger.LogInformation("Starting web host");

    WebApplicationBuilder builder = WebApplicationHelper.CreateDefaultBuilder(args);

    BindConfigurations(builder.Configuration);

    ApiDefaultServicesConfig defaultConfig = GetApiDefaultServicesConfig(
        builder.Configuration,
        WebApiConfig.Instance);

    ConfigureServices(defaultConfig, builder.Services, builder.Configuration, builder.Environment);

    ConfigureContainer(builder.Host, defaultConfig.ScanningAssemblies);

    WebApplication app = builder.Build();

    ConfigurePipeline(app, resources, defaultConfig.HangfireConfig);

    await Initialize(app, defaultConfig.HangfireConfig, defaultConfig.PubSubConfig, cancellationToken);

    app.Run();

    logger.LogInformation("Shutdown web host");
    return 0;
}
catch (Exception ex)
{
    logger.LogCritical(ex, "Host terminated unexpectedly");
    return 1;
}


// ===== METHODS =====

static ApiDefaultServicesConfig GetApiDefaultServicesConfig(
    IConfiguration configuration,
    WebApiConfig webConfig)
{
    // Common
    Type[] representativeTypes = new[]
    {
        typeof(NetTemplate.Blog.WebApi.AssemblyType),
        typeof(NetTemplate.Blog.Infrastructure.AssemblyType),
        typeof(NetTemplate.Blog.ApplicationCore.AssemblyType)
    };
    Assembly[] assemblies = representativeTypes.Select(t => t.Assembly).ToArray();
    Action<MvcOptions> controllerConfigureAction = (opt) =>
    {
        opt.CacheProfiles.Add(CacheProfiles.Sample, new CacheProfile
        {
            VaryByQueryKeys = new[] { "*" },
            Duration = webConfig.ResponseCacheTtl
        });
    };

    // DbContext
    string dbContextConnectionString = configuration.GetConnectionString(nameof(MainDbContext));

    // Hangfire
    HangfireConfig hangfireConfig = configuration.GetHangfireConfigDefaults();
    string hangfireConnStr = configuration.GetConnectionString(BackgroundConnectionNames.Hangfire);
    string masterConnStr = configuration.GetConnectionString(BackgroundConnectionNames.Master);

    // Identity
    IdentityConfig identityConfig = configuration.GetIdentityConfigDefaults();
    JwtConfig jwtConfig = configuration.GetJwtConfigDefaults();
    SimulatedAuthConfig simulatedAuthConfig = configuration.GetSimulatedAuthConfigDefaults();
    ClientsConfig clientsConfig = configuration.GetClientsConfigDefaults();

    // Client SDK
    ClientConfig clientConfig = configuration.GetClientConfigDefaults();

    // PubSubConfig
    PubSubConfig pubSubConfig = configuration.GetPubSubConfigDefaults();

    // Redis
    RedisConfig redisConfig = configuration.GetRedisConfigDefaults();

    return new ApiDefaultServicesConfig
    {
        ClientConfig = clientConfig,
        ClientsConfig = clientsConfig,
        ControllerConfigureAction = controllerConfigureAction,
        DbContextConnectionString = dbContextConnectionString,
        DbContextDebugEnabled = webConfig.DbContextDebugEnabled,
        HangfireConfig = hangfireConfig,
        HangfireConnectionString = hangfireConnStr,
        HangfireMasterConnectionString = masterConnStr,
        IdentityConfig = identityConfig,
        JwtConfig = jwtConfig,
        SimulatedAuthConfig = simulatedAuthConfig,
        PubSubConfig = pubSubConfig,
        ScanningAssemblies = assemblies,
        UseRedis = webConfig.UseRedis,
        RedisConfig = redisConfig
    };
};

static void BindConfigurations(IConfiguration configuration)
{
    configuration.GetSection(CommonConfigurationSections.App).Bind(WebApiConfig.Instance);
}

static void ConfigureServices(ApiDefaultServicesConfig defaultConfig,
    IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
{
    services
        .AddInfrastructureServices(defaultConfig, configuration, env.IsProduction())
        .AddApiDefaultServices<MainDbContext>(defaultConfig, env);
}

static void ConfigureContainer(IHostBuilder hostBuilder,
    Assembly[] scanningAssemblies)
{
    hostBuilder.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.ConfigureApiContainerDefaults(scanningAssemblies);
    });
}

static void ConfigurePipeline(WebApplication app,
    List<IDisposable> resources,
    HangfireConfig hangfireConfig)
{
    using IServiceScope scope = app.Services.CreateScope();
    IServiceProvider serviceProvider = scope.ServiceProvider;
    IApiVersionDescriptionProvider apiVersionProvider = serviceProvider.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseExceptionHandler(
        $"/{ApiRoutes.Error.Base}" +
        $"/{ApiRoutes.Error.HandleException}");

    if (app.Environment.IsProduction())
    {
        app.UseHttpsRedirection();
        app.UseHsts();
    }

    app.UseRouting();

    if (!app.Environment.IsProduction())
    {
        app.UseApplicationSwagger(apiVersionProvider);
    }

    app.UseCors(builder =>
    {
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
        builder.AllowAnyOrigin();
    });

    #region [OPTIONAL]

    app.UseResponseCaching();

    //app.UseStaticFiles();

    #endregion

    app.UseRequestBuffering();

    app.UseAuthentication();

    app.UseRequestDataExtraction();

    app.UseRequestLogging(app.Configuration,
        requestLoggingSection: WebLoggingConfigurationSections.RequestLogging,
        out IDisposable customRequestLogger);

    if (customRequestLogger != null) resources.Add(customRequestLogger);

    app.UseAuthorization();

    app.MapControllers();

    if (hangfireConfig.UseDashboard && !app.Environment.IsProduction())
    {
        app.MapHangfireDashboard();
    }

    app.Lifetime.ApplicationStarted.Register(OnApplicationStarted);
    app.Lifetime.ApplicationStopped.Register(() => OnApplicationStopped(resources));
}

static async Task Initialize(WebApplication app,
    HangfireConfig hangfireConfig,
    PubSubConfig pubSubConfig,
    CancellationToken cancellationToken = default)
{
    using IServiceScope serviceScope = app.Services.CreateScope();

    dynamic dynamicData = new ExpandoObject();
    dynamicData.HangfireConfig = hangfireConfig;
    dynamicData.PubSubConfig = pubSubConfig;

    IMediator mediator = serviceScope.ServiceProvider.GetRequiredService<IMediator>();
    await mediator.Publish(new ApplicationStartingEvent(dynamicData), cancellationToken);
}

static void OnApplicationStarted()
{
}

static void OnApplicationStopped(IEnumerable<IDisposable> resources)
{
    InfrastructureHelper.CleanResources(resources);
}