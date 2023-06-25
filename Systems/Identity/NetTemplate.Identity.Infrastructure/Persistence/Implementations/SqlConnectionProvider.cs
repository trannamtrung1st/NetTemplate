using Microsoft.Extensions.Configuration;
using NetTemplate.Common.DependencyInjection.Attributes;
using NetTemplate.Shared.Infrastructure.Persistence.Implementations;

namespace NetTemplate.Identity.Infrastructure.Persistence.Implementations
{
    [SingletonService]

    public class SqlConnectionProvider : BaseSqlConnectionProvider
    {
        public SqlConnectionProvider(IConfiguration configuration) : base(configuration)
        {
        }

        protected override string DefaultConnectionString => configuration.GetConnectionString(nameof(MainDbContext));
    }
}
