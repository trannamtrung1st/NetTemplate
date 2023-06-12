using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.FileProviders;

namespace NetTemplate.Shared.Infrastructure.Common.Extensions
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder InsertSharedJson(this IConfigurationBuilder builder)
        {
            builder.Sources.Insert(0, new JsonConfigurationSource
            {
                Path = AppsettingsSharedPath,
                Optional = true
            });

            string devSharedDir = Path.GetFullPath(DevelopmentSharedDirectory);

            if (Directory.Exists(devSharedDir))
            {
                builder.Sources.Insert(0, new JsonConfigurationSource
                {
                    Path = AppsettingsSharedPath,
                    Optional = true,
                    FileProvider = new PhysicalFileProvider(devSharedDir)
                });
            }

            return builder;
        }

        public static IConfigurationBuilder AddAppsettingsJson(this IConfigurationBuilder builder,
            bool optional = false)
        {
            return builder.AddJsonFile(AppsettingsPath, optional);
        }


        public const string DevelopmentSharedDirectory = "../Shared/";
        public const string AppsettingsPath = "appsettings.json";
        public const string AppsettingsSharedPath = "appsettings.shared.json";
    }
}
