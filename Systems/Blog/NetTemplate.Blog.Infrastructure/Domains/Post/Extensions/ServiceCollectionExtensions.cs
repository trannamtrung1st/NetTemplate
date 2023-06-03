using Microsoft.Extensions.DependencyInjection;
using NetTemplate.Blog.ApplicationCore.Post.Views;
using NetTemplate.Blog.Infrastructure.Domains.Post.Implementations;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;

namespace NetTemplate.Blog.Infrastructure.Domains.Post.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSimplePostCache(this IServiceCollection services)
        {
            return services.AddScoped<IEntityCache<PostView>, SimplePostCache>();
        }

        public static IServiceCollection AddRedisPostCache(this IServiceCollection services)
        {
            return services.AddScoped<IEntityCache<PostView>, RedisPostCache>();
        }
    }
}
