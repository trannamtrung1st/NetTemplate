using Autofac;
using Microsoft.AspNetCore.Hosting;
using NetTemplate.ApacheKafka.Extensions;
using NetTemplate.ApacheKafka.Models;
using NetTemplate.Blog.ConsumersWorker;
using NetTemplate.Blog.Infrastructure.Common.Extensions;
using NetTemplate.Blog.Infrastructure.Common.Models;
using NetTemplate.Blog.Infrastructure.Persistence;
using NetTemplate.Redis.Extensions;
using NetTemplate.Redis.Models;
using NetTemplate.Shared.ClientSDK.Common.Models;
using NetTemplate.Shared.Infrastructure.Background.Extensions;
using NetTemplate.Shared.Infrastructure.Background.Models;
using NetTemplate.Shared.Infrastructure.Common.Extensions;
using NetTemplate.Shared.Infrastructure.Common.Utils;
using NetTemplate.Shared.Infrastructure.Identity.Extensions;
using NetTemplate.Shared.Infrastructure.Identity.Models;
using Serilog.Extensions.Logging;
using System.Reflection;
using BackgroundConnectionNames = NetTemplate.Shared.Infrastructure.Background.Constants.ConnectionNames;


// ===== APPLICATION START =====

#if DEBUG
bool isProduction = false;
#else
bool isProduction = true;
#endif

using Serilog.Core.Logger seriLogger = InfrastructureHelper.CreateHostLogger(isProduction);
ILogger logger = new SerilogLoggerFactory(seriLogger).CreateLogger(nameof(Program));

try
{
    logger.LogInformation("Starting host");

    List<IDisposable> resources = new List<IDisposable>();
    using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
    CancellationToken cancellationToken = cancellationTokenSource.Token;

    IHostBuilder builder = HostHelper.CreateDefaultBuilder(args);

    builder.ConfigureServices((context, services) =>
    {
        ParseConfigurations(context.Configuration);
        ConfigureServices(services, context.Configuration, context.HostingEnvironment);
        ConfigureContainer(builder, RuntimeConfig.ScanningAssemblies);
    });

    IHost host = builder.Build();

    await host.RunAsync();

    logger.LogInformation("Shutdown host");
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
    AppConfig = configuration.GetApplicationConfigDefaults<ApplicationConfig>();

    // DbContext
    DbContextConnectionString = configuration.GetConnectionString(nameof(MainDbContext));

    // Hangfire
    HangfireConfig = configuration.GetHangfireConfigDefaults();
    HangfireConnectionString = configuration.GetConnectionString(BackgroundConnectionNames.Hangfire);
    HangfireMasterConnectionString = configuration.GetConnectionString(BackgroundConnectionNames.Master);

    // Identity
    IdentityConfig = configuration.GetIdentityConfigDefaults();

    // Client SDK
    ClientConfig = configuration.GetClientConfigDefaults();

    // Redis
    RedisConfig = configuration.GetRedisConfigDefaults();
    RedisPubSubConfig = configuration.GetRedisPubSubConfigDefaults();

    // Apache Kafka
    ApacheKafkaConfig = configuration.GetApacheKafkaConfigDefaults();
}

static RuntimeConfig GetRuntimeConfig()
{
    // Common
    Type[] representativeTypes = new[]
    {
        typeof(Program),
        typeof(NetTemplate.Blog.Infrastructure.AssemblyType),
        typeof(NetTemplate.Blog.ApplicationCore.AssemblyType)
    };
    Assembly[] assemblies = representativeTypes.Select(t => t.Assembly).ToArray();

    return new RuntimeConfig
    {
        ScanningAssemblies = assemblies
    };
}

static void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
{
    services.AddHostedService<Worker>();

    services.AddInfrastructureServices(configuration, env.IsProduction(),
        RuntimeConfig, AppConfig,
        DbContextConnectionString,
        IdentityConfig,
        HangfireConfig, HangfireConnectionString, HangfireMasterConnectionString,
        RedisConfig, RedisPubSubConfig,
        ClientConfig,
        ApacheKafkaConfig);
}

static void ConfigureContainer(IHostBuilder hostBuilder,
    Assembly[] scanningAssemblies)
{
    hostBuilder.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.ConfigureContainerDefaults(scanningAssemblies);
    });
}

partial class Program
{
    static RuntimeConfig RuntimeConfig { get; set; }
    static ApplicationConfig AppConfig { get; set; }
    static string DbContextConnectionString { get; set; }
    static HangfireConfig HangfireConfig { get; set; }
    static string HangfireConnectionString { get; set; }
    static string HangfireMasterConnectionString { get; set; }
    static IdentityConfig IdentityConfig { get; set; }
    static ClientConfig ClientConfig { get; set; }
    static RedisConfig RedisConfig { get; set; }
    static ApacheKafkaConfig ApacheKafkaConfig { get; set; }
    static RedisPubSubConfig RedisPubSubConfig { get; set; }
}