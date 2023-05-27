using Autofac;
using NetTemplate.Shared.Infrastructure.Common.Extensions;
using System.Reflection;

namespace NetTemplate.Shared.WebApi.Common.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static void ConfigureApiContainerDefaults(this ContainerBuilder builder,
            Assembly[] scanningAssemblies)
        {
            builder.ConfigureContainerDefaults(scanningAssemblies);

            // [NOTE] others
        }
    }
}
