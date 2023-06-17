using Autofac;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using NetTemplate.ApacheKafka.Extensions;
using NetTemplate.ApacheKafka.Models;
using NetTemplate.Blog.ApplicationCore.Common.Extensions;
using NetTemplate.Blog.ApplicationCore.Common.Models;
using NetTemplate.Blog.Infrastructure.Common.Extensions;
using NetTemplate.Blog.Infrastructure.Common.Models;
using NetTemplate.Blog.Infrastructure.Persistence;
using NetTemplate.Blog.Infrastructure.PubSub.Extensions;
using NetTemplate.Blog.Infrastructure.PubSub.Models;
using NetTemplate.Blog.WebApi.Common.Extensions;
using NetTemplate.Blog.WebApi.Common.Models;
using NetTemplate.Common.Logging.Options;
using NetTemplate.Common.Web.Middleware.Implementations;
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
using NetTemplate.Shared.WebApi.Common.Extensions;
using NetTemplate.Shared.WebApi.Common.Utils;
using NetTemplate.Shared.WebApi.Identity.Extensions;
using NetTemplate.Shared.WebApi.Identity.Models;
using NetTemplate.Shared.WebApi.Logging.Extensions;
using NetTemplate.Shared.WebApi.Swagger.Extensions;
using Serilog.Extensions.Logging;
using System.Reflection;
using ApiRoutes = NetTemplate.Blog.WebApi.Common.Constants.ApiRoutes;
using BackgroundConnectionNames = NetTemplate.Shared.Infrastructure.Background.Constants.ConnectionNames;
using CacheProfiles = NetTemplate.Blog.WebApi.Common.Constants.CacheProfiles;


// ===== APPLICATION START =====

bool isProduction = WebApplicationHelper.IsProduction();
using Serilog.Core.Logger seriLogger = InfrastructureHelper.CreateHostLogger(isProduction);
ILogger logger = new SerilogLoggerFactory(seriLogger).CreateLogger(nameof(Program));

try
{
    logger.LogInformation("Starting web host");

    List<IDisposable> resources = new List<IDisposable>();
    using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
    CancellationToken cancellationToken = cancellationTokenSource.Token;

    WebApplicationBuilder builder = WebApplicationHelper.CreateDefaultBuilder(args);
    IConfiguration configuration = builder.Configuration;

    ParseConfigurations(configuration);

    ConfigureServices(builder.Services, builder.Configuration, builder.Environment);

    ConfigureContainer(builder.Host, RuntimeConfig.ScanningAssemblies);

    WebApplication app = builder.Build();

    ConfigurePipeline(app, resources);

    await Initialize(app, cancellationToken);

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

static void ParseConfigurations(IConfiguration configuration)
{
    // Common
    RuntimeConfig = GetRuntimeConfig();
    WebConfig = configuration.GetApplicationConfigDefaults<WebApplicationConfig>();
    ControllerConfigureAction = (opt) =>
    {
        opt.CacheProfiles.Add(CacheProfiles.Sample, new CacheProfile
        {
            VaryByQueryKeys = new[] { "*" },
            Duration = WebConfig.ResponseCacheTtl
        });
    };
    ViewsConfig = configuration.GetViewsConfigDefaults();

    // DbContext
    DbContextConnectionString = configuration.GetConnectionString(nameof(MainDbContext));

    // Hangfire
    HangfireConfig = configuration.GetHangfireConfigDefaults();
    HangfireConnectionString = configuration.GetConnectionString(BackgroundConnectionNames.Hangfire);
    HangfireMasterConnectionString = configuration.GetConnectionString(BackgroundConnectionNames.Master);

    // Identity
    IdentityConfig = configuration.GetIdentityConfigDefaults();
    JwtConfig = configuration.GetJwtConfigDefaults();
    SimulatedAuthConfig = configuration.GetSimulatedAuthConfigDefaults();
    ApplicationClientsConfig = configuration.GetClientsConfigDefaults();

    // Client SDK
    ClientConfig = configuration.GetClientConfigDefaults();

    // Redis
    RedisConfig = configuration.GetRedisConfigDefaults();
    RedisPubSubConfig = configuration.GetRedisPubSubConfigDefaults();

    // Apache Kafka
    ApacheKafkaConfig = configuration.GetApacheKafkaConfigDefaults();

    // PubSub
    PubSubConfig = configuration.GetPubSubConfigDefaults();
}

static RuntimeConfig GetRuntimeConfig()
{
    // Common
    Type[] representativeTypes = new[]
    {
        typeof(NetTemplate.Blog.WebApi.AssemblyType),
        typeof(NetTemplate.Blog.Infrastructure.AssemblyType),
        typeof(NetTemplate.Blog.ApplicationCore.AssemblyType)
    };
    Assembly[] assemblies = representativeTypes.Select(t => t.Assembly).ToArray();

    return new RuntimeConfig
    {
        ScanningAssemblies = assemblies
    };
}

static void ConfigureServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
{
    services.AddInfrastructureServices(configuration, env.IsProduction(),
        RuntimeConfig, WebConfig,
        DbContextConnectionString,
        IdentityConfig,
        HangfireConfig, HangfireConnectionString, HangfireMasterConnectionString,
        RedisConfig, RedisPubSubConfig,
        ClientConfig,
        ApacheKafkaConfig,
        PubSubConfig,
        ViewsConfig);

    services.AddApiServices(env,
        WebConfig,
        JwtConfig,
        ApplicationClientsConfig,
        SimulatedAuthConfig,
        ControllerConfigureAction);
}

static void ConfigureContainer(IHostBuilder hostBuilder,
    Assembly[] scanningAssemblies)
{
    hostBuilder.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.ConfigureApiContainerDefaults(scanningAssemblies);
    });
}

static void ConfigurePipeline(WebApplication app, List<IDisposable> resources)
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
        requestLoggingSection: RequestLoggingOptions.ConfigurationSection,
        out IDisposable customRequestLogger);

    if (customRequestLogger != null) resources.Add(customRequestLogger);

    app.UseAuthorization();

    app.MapControllers();

    if (HangfireConfig.UseDashboard && !app.Environment.IsProduction())
    {
        app.MapHangfireDashboard();
    }

    app.Lifetime.ApplicationStarted.Register(OnApplicationStarted);
    app.Lifetime.ApplicationStopped.Register(() => OnApplicationStopped(resources));
}

static async Task Initialize(WebApplication app,
    CancellationToken cancellationToken = default)
{
    using IServiceScope serviceScope = app.Services.CreateScope();

    IMediator mediator = serviceScope.ServiceProvider.GetRequiredService<IMediator>();

    await mediator.Publish(new ApplicationStartingEvent(), cancellationToken);
}

static void OnApplicationStarted()
{
}

static void OnApplicationStopped(IEnumerable<IDisposable> resources)
{
    InfrastructureHelper.CleanResources(resources);
}

partial class Program
{
    static RuntimeConfig RuntimeConfig { get; set; }
    static WebApplicationConfig WebConfig { get; set; }
    static Action<MvcOptions> ControllerConfigureAction { get; set; }
    static string DbContextConnectionString { get; set; }
    static HangfireConfig HangfireConfig { get; set; }
    static string HangfireConnectionString { get; set; }
    static string HangfireMasterConnectionString { get; set; }
    static IdentityConfig IdentityConfig { get; set; }
    static JwtConfig JwtConfig { get; set; }
    static SimulatedAuthConfig SimulatedAuthConfig { get; set; }
    static ApplicationClientsConfig ApplicationClientsConfig { get; set; }
    static ClientConfig ClientConfig { get; set; }
    static RedisConfig RedisConfig { get; set; }
    static ApacheKafkaConfig ApacheKafkaConfig { get; set; }
    static RedisPubSubConfig RedisPubSubConfig { get; set; }
    static PubSubConfig PubSubConfig { get; set; }
    static ViewsConfig ViewsConfig { get; set; }
}