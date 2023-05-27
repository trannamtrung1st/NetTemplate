using Autofac;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using NetTemplate.Blog.ApplicationCore.Cross;
using NetTemplate.Blog.Infrastructure.Persistence;
using NetTemplate.Blog.WebApi.Common;
using NetTemplate.Blog.WebApi.Common.Models;
using NetTemplate.Common.Web.Middlewares;
using NetTemplate.Shared.ApplicationCore.Common.Events;
using NetTemplate.Shared.ClientSDK.Common.Models;
using NetTemplate.Shared.Infrastructure.Background;
using NetTemplate.Shared.Infrastructure.Background.Extensions;
using NetTemplate.Shared.Infrastructure.Background.Models;
using NetTemplate.Shared.Infrastructure.Common.Extensions;
using NetTemplate.Shared.Infrastructure.Identity.Extensions;
using NetTemplate.Shared.Infrastructure.Identity.Models;
using NetTemplate.Shared.Infrastructure.PubSub.Extensions;
using NetTemplate.Shared.Infrastructure.PubSub.Models;
using NetTemplate.Shared.WebApi.Common.Constants;
using NetTemplate.Shared.WebApi.Common.Extensions;
using NetTemplate.Shared.WebApi.Common.Models;
using NetTemplate.Shared.WebApi.Common.Utils;
using NetTemplate.Shared.WebApi.Identity.Extensions;
using NetTemplate.Shared.WebApi.Identity.Models;
using NetTemplate.Shared.WebApi.Logging.Extensions;
using NetTemplate.Shared.WebApi.Swagger.Extensions;
using Newtonsoft.Json;
using Serilog.Extensions.Logging;
using System.Reflection;

using Serilog.Core.Logger seriLogger = WebApplicationHelper.CreateHostLogger();
ILogger logger = new SerilogLoggerFactory(seriLogger).CreateLogger(nameof(Program));
List<IDisposable> resources = new List<IDisposable>();

try
{
    logger.LogInformation("Starting web host");

    WebApplicationBuilder builder = WebApplicationHelper.CreateBuilder(args);

    BindConfigurations(builder.Configuration);

    DefaultServicesConfiguration defaultConfig = GetDefaultServicesConfiguration(
        builder.Configuration,
        AppConfig.Instance);

    ConfigureServices(builder.Services, builder.Environment, defaultConfig);

    ConfigureContainer(builder.Host, defaultConfig.ScanningAssemblies);

    WebApplication app = builder.Build();

    ConfigurePipeline(app, resources, defaultConfig.HangfireConfig);

    await Initialize(app);

    RunJobs(app, defaultConfig.HangfireConfig);

    StartConsumers(app);

    app.Run();

    logger.LogInformation("Shutdown web host");
    return 0;
}
catch (Exception ex)
{
    logger.LogCritical(ex, "Host terminated unexpectedly");
    return 1;
}

static DefaultServicesConfiguration GetDefaultServicesConfiguration(
    IConfiguration configuration,
    AppConfig appConfig)
{
    ClientConfiguration clientConfiguration = configuration.GetClientConfigurationDefaults();

    ClientsConfig clientsConfig = configuration.GetClientsConfigDefaults();

    string dbContextConnectionString = configuration.GetConnectionString(nameof(MainDbContext));

    HangfireConfig hangfireConfig = configuration.GetHangfireConfigDefaults();
    string hangfireConnStr = configuration.GetConnectionString(BackgroundConstants.DefaultConnectionNames.Hangfire);
    string masterConnStr = configuration.GetConnectionString(BackgroundConstants.DefaultConnectionNames.Master);

    IdentityConfig identityConfig = configuration.GetIdentityConfigDefaults();

    JwtConfig jwtConfig = configuration.GetJwtConfigDefaults();

    PubSubConfig pubSubConfig = configuration.GetPubSubConfigDefaults();

    Type[] representativeTypes = new[]
    {
        typeof(NetTemplate.Blog.WebApi.AssemblyType),
        typeof(NetTemplate.Blog.Infrastructure.AssemblyType),
        typeof(NetTemplate.Blog.Infrastructure.Domains.AssemblyType),
        typeof(NetTemplate.Blog.ApplicationCore.AssemblyType)
    };
    Assembly[] assemblies = representativeTypes.Select(t => t.Assembly).ToArray();

    return new DefaultServicesConfiguration
    {
        ClientConfiguration = clientConfiguration,
        ClientsConfig = clientsConfig,
        ControllerConfigureAction = (opt) =>
        {
            opt.CacheProfiles.Add(ApiCommonConstants.CacheProfiles.Sample, new CacheProfile
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
        PubSubConfig = pubSubConfig,
        ScanningAssemblies = assemblies
    };
};

static void BindConfigurations(IConfiguration configuration)
{
    configuration.Bind(AppConfig.Instance);
}

static void ConfigureServices(IServiceCollection services,
    IWebHostEnvironment env, DefaultServicesConfiguration defaultConfig)
{
    services.AddDefaultServices<MainDbContext>(env, defaultConfig);
}

static void ConfigureContainer(IHostBuilder hostBuilder,
    Assembly[] scanningAssemblies)
{
    hostBuilder.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.ConfigureAutofacContainerDefaults(scanningAssemblies);
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
        $"/{ApiCommonConstants.ApiRoutes.Error.Base}" +
        $"/{ApiCommonConstants.ApiRoutes.Error.HandleException}");

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
        requestLoggingSection: SharedApiConstants.ConfigKeys.Logging.RequestLogging,
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

static async Task Initialize(WebApplication app)
{
    using IServiceScope serviceScope = app.Services.CreateScope();

    // Database
    MainDbContext context = serviceScope.ServiceProvider.GetRequiredService<MainDbContext>();
    await context.Database.MigrateAsync();
    await context.SeedMigrationsAsync(serviceScope.ServiceProvider);

    IMediator mediator = serviceScope.ServiceProvider.GetRequiredService<IMediator>();
    await mediator.Publish(new ApplicationStartingEvent());
}

static void RunJobs(WebApplication app, HangfireConfig config)
{
    IEnumerable<CronJob> jobs = config.Jobs;

    if (jobs?.Any() != true) return;

    using var serviceScope = app.Services.CreateScope();
    var recurringJobManager = serviceScope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
    var defaultTimeZone = config.TimeZoneInfo;

    foreach (var job in config.Jobs)
    {
        var count = 1;
        foreach (var cronExpr in job.CronExpressions)
        {
            switch (job.Name)
            {
                case CrossConstants.JobNames.Sample:
                    {
                        var serializedData = JsonConvert.SerializeObject(job.JobData);
                        var jobData = JsonConvert.DeserializeObject(serializedData);
                        var finalName = CrossConstants.JobNames.Sample + (count++);

                        recurringJobManager.AddOrUpdate(finalName,
                            () => Console.WriteLine("Sample Job Run"),
                            cronExpr,
                            new RecurringJobOptions { TimeZone = defaultTimeZone });
                        break;
                    }
            }
        }
    }
}

static void StartConsumers(WebApplication app)
{
    // [TODO]
}

static void OnApplicationStarted()
{
}

static void OnApplicationStopped(IEnumerable<IDisposable> resources)
{
    StartupHelper.CleanResources(resources);
}