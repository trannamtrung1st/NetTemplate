using Microsoft.Extensions.DependencyInjection;
using NetTemplate.Shared.ApplicationCore.Common.Implementations;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;

namespace NetTemplate.Shared.ApplicationCore.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityVersionManager(this IServiceCollection services)
        {
            return services.AddScoped<IEntityVersionManager, EntityVersionManager>();
        }
    }
}
