using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetTemplate.Blog.ConsoleApp.UseCases;
using NetTemplate.Blog.Infrastructure.Common.Extensions;
using NetTemplate.Blog.Infrastructure.Common.Models;
using NetTemplate.Blog.Infrastructure.Persistence;
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
using BackgroundConnectionNames = NetTemplate.Shared.Infrastructure.Background.Constants.ConnectionNames;
using CommonConfigurationSections = NetTemplate.Blog.ApplicationCore.Common.Constants.ConfigurationSections;

// ===== APPLICATION START =====

List<IDisposable> resources = new List<IDisposable>();

IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
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

await InsertLargeData.Run(serviceProvider);


// ===== METHODS =====

static DefaultServicesConfig GetDefaultServicesConfig(
    IConfiguration configuration,
    InfrastructureConfig infrasConfig)
{
    string dbContextConnectionString = configuration.GetConnectionString(nameof(MainDbContext));

    HangfireConfig hangfireConfig = configuration.GetHangfireConfigDefaults();
    string hangfireConnStr = configuration.GetConnectionString(BackgroundConnectionNames.Hangfire);
    string masterConnStr = configuration.GetConnectionString(BackgroundConnectionNames.Master);

    IdentityConfig identityConfig = configuration.GetIdentityConfigDefaults();
    ClientConfig clientConfiguration = configuration.GetClientConfigDefaults();

    PubSubConfig pubSubConfig = configuration.GetPubSubConfigDefaults();

    Type[] representativeTypes = new[]
    {
        typeof(NetTemplate.Blog.Infrastructure.AssemblyType),
        typeof(NetTemplate.Blog.Infrastructure.Domains.AssemblyType),
        typeof(NetTemplate.Blog.ApplicationCore.AssemblyType)
    };
    Assembly[] assemblies = representativeTypes.Select(t => t.Assembly).ToArray();

    return new DefaultServicesConfig
    {
        ClientConfig = clientConfiguration,
        DbContextConnectionString = dbContextConnectionString,
        DbContextDebugEnabled = infrasConfig.DbContextDebugEnabled,
        HangfireConfig = hangfireConfig,
        HangfireConnectionString = hangfireConnStr,
        HangfireMasterConnectionString = masterConnStr,
        IdentityConfig = identityConfig,
        PubSubConfig = pubSubConfig,
        ScanningAssemblies = assemblies
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
