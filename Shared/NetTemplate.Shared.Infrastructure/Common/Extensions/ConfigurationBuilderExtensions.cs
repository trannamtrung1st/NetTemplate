using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.FileProviders;
using DefaultPaths = NetTemplate.Shared.Infrastructure.Common.Constants.DefaultPaths;

namespace NetTemplate.Shared.Infrastructure.Common.Extensions
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder InsertSharedJson(this IConfigurationBuilder builder)
        {
            builder.Sources.Insert(0, new JsonConfigurationSource
            {
                Path = DefaultPaths.AppsettingsSharedPath,
                Optional = true
            });

            string devSharedDir = Path.GetFullPath(DefaultPaths.DevelopmentSharedDirectory);

            if (Directory.Exists(devSharedDir))
            {
                builder.Sources.Insert(0, new JsonConfigurationSource
                {
                    Path = DefaultPaths.AppsettingsSharedPath,
                    Optional = true,
                    FileProvider = new PhysicalFileProvider(devSharedDir)
                });
            }

            return builder;
        }
    }
}
