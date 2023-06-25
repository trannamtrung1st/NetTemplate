using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NetTemplate.Shared.Infrastructure.Persistence.Interfaces;

namespace NetTemplate.Shared.Infrastructure.Persistence.Implementations
{
    public abstract class BaseSqlConnectionProvider : ISqlConnectionProvider
    {
        protected readonly IConfiguration configuration;

        public BaseSqlConnectionProvider(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected abstract string DefaultConnectionString { get; }

        public SqlConnection CreateConnection(string connectionString = default)
        {
            connectionString ??= DefaultConnectionString;

            return new SqlConnection(connectionString);
        }
    }
}
