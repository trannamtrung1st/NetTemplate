using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetTemplate.Blog.ConsoleApp.UseCases;
using NetTemplate.Blog.Infrastructure.Common.Extensions;
using NetTemplate.Blog.Infrastructure.Common.Models;
using NetTemplate.Blog.Infrastructure.Persistence;
using NetTemplate.Redis.Extensions;
using NetTemplate.Redis.Models;
using NetTemplate.Shared.ClientSDK.Common.Models;
using NetTemplate.Shared.Infrastructure.Background.Extensions;
using NetTemplate.Shared.Infrastructure.Background.Models;
using NetTemplate.Shared.Infrastructure.Common.Extensions;
using NetTemplate.Shared.Infrastructure.Common.Models;
using NetTemplate.Shared.Infrastructure.Identity.Extensions;
using NetTemplate.Shared.Infrastructure.Identity.Models;
using NetTemplate.Shared.Infrastructure.PubSub.Extensions;
using NetTemplate.Shared.Infrastructure.PubSub.Models;
using System.Reflection;
using static NetTemplate.Shared.Infrastructure.Common.Constants;
using BackgroundConnectionNames = NetTemplate.Shared.Infrastructure.Background.Constants.ConnectionNames;
using CommonConfigurationSections = NetTemplate.Blog.ApplicationCore.Common.Constants.ConfigurationSections;

// ===== APPLICATION START =====

using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
List<IDisposable> resources = new List<IDisposable>();
CancellationToken cancellationToken = cancellationTokenSource.Token;

IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
    .InsertSharedJson()
    .AddJsonFile(DefaultPaths.AppsettingsPath, optional: false)
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .AddUserSecrets<Program>();

InfrastructureConfig infrasConfig = new InfrastructureConfig();
IConfigurationRoot configuration = configurationBuilder.Build();
IServiceCollection services = new ServiceCollection();
ContainerBuilder containerBuilder = new ContainerBuilder();

BindConfigurations(configuration, infrasConfig);

DefaultServicesConfig defaultConfig = GetDefaultServicesConfig(
    configuration,
    infrasConfig);

ConfigureServices(defaultConfig, services, configuration);

IContainer container = ConfigureContainer(containerBuilder, services, defaultConfig.ScanningAssemblies);

IServiceProvider serviceProvider = new AutofacServiceProvider(container);

await InsertLargeData.Run(serviceProvider, cancellationToken);


// ===== METHODS =====

static DefaultServicesConfig GetDefaultServicesConfig(
    IConfiguration configuration,
    InfrastructureConfig infrasConfig)
{
    // Common
    Type[] representativeTypes = new[]
    {
        typeof(NetTemplate.Blog.Infrastructure.AssemblyType),
        typeof(NetTemplate.Blog.ApplicationCore.AssemblyType)
    };
    Assembly[] assemblies = representativeTypes.Select(t => t.Assembly).ToArray();

    // DbContext
    string dbContextConnectionString = configuration.GetConnectionString(nameof(MainDbContext));

    // Hangfire
    HangfireConfig hangfireConfig = configuration.GetHangfireConfigDefaults();
    string hangfireConnStr = configuration.GetConnectionString(BackgroundConnectionNames.Hangfire);
    string masterConnStr = configuration.GetConnectionString(BackgroundConnectionNames.Master);

    // Identity
    IdentityConfig identityConfig = configuration.GetIdentityConfigDefaults();

    // Client SDK
    ClientConfig clientConfig = configuration.GetClientConfigDefaults();

    // PubSubConfig
    PubSubConfig pubSubConfig = configuration.GetPubSubConfigDefaults();

    // Redis
    RedisConfig redisConfig = configuration.GetRedisConfigDefaults();

    return new DefaultServicesConfig
    {
        ClientConfig = clientConfig,
        DbContextConnectionString = dbContextConnectionString,
        DbContextDebugEnabled = infrasConfig.DbContextDebugEnabled,
        HangfireConfig = hangfireConfig,
        HangfireConnectionString = hangfireConnStr,
        HangfireMasterConnectionString = masterConnStr,
        IdentityConfig = identityConfig,
        PubSubConfig = pubSubConfig,
        ScanningAssemblies = assemblies,
        UseRedis = infrasConfig.UseRedis,
        RedisConfig = redisConfig
    };
};

static void BindConfigurations(IConfiguration configuration,
    InfrastructureConfig infrasConfig)
{
    configuration.GetSection(CommonConfigurationSections.App).Bind(infrasConfig);
}

static void ConfigureServices(DefaultServicesConfig defaultConfig,
    IServiceCollection services, IConfiguration configuration)
{
    services
        .AddInfrastructureServices(defaultConfig, configuration, isProduction: false);
}

static IContainer ConfigureContainer(ContainerBuilder containerBuilder,
    IServiceCollection services,
    Assembly[] scanningAssemblies)
{
    containerBuilder.ConfigureContainerDefaults(scanningAssemblies);

    containerBuilder.Populate(services);

    return containerBuilder.Build();
}
