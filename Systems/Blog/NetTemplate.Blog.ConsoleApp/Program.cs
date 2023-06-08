using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
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
using NetTemplate.Shared.Infrastructure.Identity.Extensions;
using NetTemplate.Shared.Infrastructure.Identity.Models;
using NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Extensions;
using NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Models;
using System.Reflection;
using static NetTemplate.Shared.Infrastructure.Common.Constants;
using BackgroundConnectionNames = NetTemplate.Shared.Infrastructure.Background.Constants.ConnectionNames;


// ===== APPLICATION START =====

List<IDisposable> resources = new List<IDisposable>();
using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
CancellationToken cancellationToken = cancellationTokenSource.Token;

IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
    .InsertSharedJson()
    .AddJsonFile(DefaultPaths.AppsettingsPath, optional: false)
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .AddUserSecrets<Program>();

IConfigurationRoot configuration = configurationBuilder.Build();
IServiceCollection services = new ServiceCollection();
ContainerBuilder containerBuilder = new ContainerBuilder();

ParseConfigurations(configuration);

ConfigureServices(services, configuration);

IContainer container = ConfigureContainer(containerBuilder, services);

IServiceProvider serviceProvider = new AutofacServiceProvider(container);

await Start(serviceProvider, cancellationToken);

// ===== METHODS =====

static async Task Start(IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
{
    string option = string.Empty;

    while (!string.Equals(option, "E", StringComparison.OrdinalIgnoreCase))
    {
        Console.Clear();
        Console.WriteLine("1. Insert large data");
        Console.WriteLine("2. Simulate identity user created");
        Console.WriteLine("E. Exit");
        Console.Write("Choose: ");
        option = Console.ReadLine()?.Trim() ?? string.Empty;

        switch (option)
        {
            case "1": await InsertLargeData.Run(serviceProvider, cancellationToken); break;

            case "2": await SimulateIdentityUserCreated.Run(serviceProvider, cancellationToken); break;
        }

        Console.WriteLine("Press enter to continue!");
        Console.ReadLine();
    }
}

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

    // Apache Kafka
    ApacheKafkaConfig = configuration.GetApacheKafkaConfigDefaults();
}

static RuntimeConfig GetRuntimeConfig()
{
    // Common
    Type[] representativeTypes = new[]
    {
        typeof(NetTemplate.Blog.Infrastructure.AssemblyType),
        typeof(NetTemplate.Blog.ApplicationCore.AssemblyType)
    };
    Assembly[] assemblies = representativeTypes.Select(t => t.Assembly).ToArray();

    return new RuntimeConfig
    {
        ScanningAssemblies = assemblies
    };
}

static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddSingleton(configuration);

    services.AddInfrastructureServices(configuration, isProduction: false,
        RuntimeConfig, AppConfig,
        DbContextConnectionString,
        IdentityConfig,
        HangfireConfig, HangfireConnectionString, HangfireMasterConnectionString,
        RedisConfig,
        ClientConfig,
        ApacheKafkaConfig);
}

static IContainer ConfigureContainer(ContainerBuilder containerBuilder,
    IServiceCollection services)
{
    containerBuilder.ConfigureContainerDefaults(RuntimeConfig.ScanningAssemblies);

    containerBuilder.Populate(services);

    return containerBuilder.Build();
}

partial class Program
{
    static RuntimeConfig RuntimeConfig { get; set; }
    static ApplicationConfig AppConfig { get; set; }
    static Action<MvcOptions> ControllerConfigureAction { get; set; }
    static string DbContextConnectionString { get; set; }
    static HangfireConfig HangfireConfig { get; set; }
    static string HangfireConnectionString { get; set; }
    static string HangfireMasterConnectionString { get; set; }
    static IdentityConfig IdentityConfig { get; set; }
    static ClientConfig ClientConfig { get; set; }
    static RedisConfig RedisConfig { get; set; }
    static ApacheKafkaConfig ApacheKafkaConfig { get; set; }
}