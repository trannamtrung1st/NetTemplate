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
using NetTemplate.Shared.ApplicationCore.Common.Events;
using NetTemplate.Shared.ClientSDK.Common.Models;
using NetTemplate.Shared.Infrastructure.Background.Extensions;
using NetTemplate.Shared.Infrastructure.Background.Models;
using NetTemplate.Shared.Infrastructure.Common.Extensions;
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
using LoggingConfigurationSections = NetTemplate.Shared.WebApi.Logging.Constants.ConfigurationSections;

// ===== APPLICATION START =====

using Serilog.Core.Logger seriLogger = WebApplicationHelper.CreateHostLogger();
ILogger logger = new SerilogLoggerFactory(seriLogger).CreateLogger(nameof(Program));
List<IDisposable> resources = new List<IDisposable>();

try
{
    logger.LogInformation("Starting web host");

    WebApplicationBuilder builder = WebApplicationHelper.CreateBuilder(args);

    BindConfigurations(builder.Configuration);

    ApiDefaultServicesConfig defaultConfig = GetApiDefaultServicesConfig(
        builder.Configuration,
        AppConfig.Instance);

    ConfigureServices(builder.Services, defaultConfig, builder.Environment, builder.Configuration);

    ConfigureContainer(builder.Host, defaultConfig.ScanningAssemblies);

    WebApplication app = builder.Build();

    ConfigurePipeline(app, resources, defaultConfig.HangfireConfig);

    await Initialize(app, defaultConfig.HangfireConfig);

    app.Run();

    logger.LogInformation("Shutdown web host");
    return 0;
}
catch (Exception ex)
{
    logger.LogCritical(ex, "Host terminated unexpectedly");
    return 1;
}

static ApiDefaultServicesConfig GetApiDefaultServicesConfig(
    IConfiguration configuration,
    AppConfig appConfig)
{
    ClientConfig clientConfiguration = configuration.GetClientConfigDefaults();
    ClientsConfig clientsConfig = configuration.GetClientsConfigDefaults();

    string dbContextConnectionString = configuration.GetConnectionString(nameof(MainDbContext));

    HangfireConfig hangfireConfig = configuration.GetHangfireConfigDefaults();
    string hangfireConnStr = configuration.GetConnectionString(BackgroundConnectionNames.Hangfire);
    string masterConnStr = configuration.GetConnectionString(BackgroundConnectionNames.Master);

    IdentityConfig identityConfig = configuration.GetIdentityConfigDefaults();
    JwtConfig jwtConfig = configuration.GetJwtConfigDefaults();
    SimulatedAuthConfig simulatedAuthConfig = configuration.GetSimulatedAuthConfigDefaults();

    PubSubConfig pubSubConfig = configuration.GetPubSubConfigDefaults();

    Type[] representativeTypes = new[]
    {
        typeof(NetTemplate.Blog.WebApi.AssemblyType),
        typeof(NetTemplate.Blog.Infrastructure.AssemblyType),
        typeof(NetTemplate.Blog.Infrastructure.Domains.AssemblyType),
        typeof(NetTemplate.Blog.ApplicationCore.AssemblyType)
    };
    Assembly[] assemblies = representativeTypes.Select(t => t.Assembly).ToArray();

    return new ApiDefaultServicesConfig
    {
        ClientConfig = clientConfiguration,
        ClientsConfig = clientsConfig,
        ControllerConfigureAction = (opt) =>
        {
            opt.CacheProfiles.Add(CacheProfiles.Sample, new CacheProfile
            {
                VaryByQueryKeys = new[] { "*" },
                Duration = appConfig.ResponseCacheTtl
            });
        },
        DbContextConnectionString = dbContextConnectionString,
        DbContextDebugEnabled = appConfig.DbContextDebugEnabled,
        HangfireConfig = hangfireConfig,
        HangfireConnectionString = hangfireConnStr,
        HangfireMasterConnectionString = masterConnStr,
        IdentityConfig = identityConfig,
        JwtConfig = jwtConfig,
        SimulatedAuthConfig = simulatedAuthConfig,
        PubSubConfig = pubSubConfig,
        ScanningAssemblies = assemblies
    };
};

static void BindConfigurations(IConfiguration configuration)
{
    configuration.Bind(AppConfig.Instance);
}

static void ConfigureServices(IServiceCollection services,
    ApiDefaultServicesConfig defaultConfig,
    IWebHostEnvironment env, IConfiguration configuration)
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
        requestLoggingSection: LoggingConfigurationSections.RequestLogging,
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
    HangfireConfig hangfireConfig)
{
    using IServiceScope serviceScope = app.Services.CreateScope();

    dynamic dynamicData = new ExpandoObject();
    dynamicData.HangfireConfig = hangfireConfig;

    IMediator mediator = serviceScope.ServiceProvider.GetRequiredService<IMediator>();
    await mediator.Publish(new ApplicationStartingEvent(dynamicData));
}

static void OnApplicationStarted()
{
}

static void OnApplicationStopped(IEnumerable<IDisposable> resources)
{
    WebApplicationHelper.CleanResources(resources);
}