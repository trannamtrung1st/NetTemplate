using NetTemplate.Shared.Infrastructure.Common.Extensions;
using EnvironmentConstants = NetTemplate.Shared.WebApi.Common.Constants.Environment;

namespace NetTemplate.Shared.WebApi.Common.Utils
{
    public static class WebApplicationHelper
    {
        public static WebApplicationBuilder CreateDefaultBuilder(string[] args)
        {
            WebApplicationBuilder webBuilder = WebApplication.CreateBuilder(args);

            webBuilder.Host.ConfigureAppConfiguration(builder => builder.InsertSharedJson());

            webBuilder.Host.UseDefault();

            return webBuilder;
        }

        public static bool IsProduction()
        {
            string env = GetEnvironmentStage();
            return env == EnvironmentConstants.Production;
        }

        public static bool IsDevelopment()
        {
            string env = GetEnvironmentStage();
            return env == EnvironmentConstants.Development;
        }

        public static string GetEnvironmentStage() => Environment.GetEnvironmentVariable(EnvironmentConstants.VariableName);
    }
}
