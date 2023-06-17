using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NetTemplate.Common.DependencyInjection.Attributes;
using NetTemplate.Shared.Infrastructure.Persistence.Interfaces;

namespace NetTemplate.Blog.Infrastructure.Persistence.Implementations
{
    [SingletonService]
    public class SqlConnectionProvider : ISqlConnectionProvider
    {
        private IConfiguration _configuration;

        public SqlConnectionProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SqlConnection CreateConnection()
        {
            string connStr = _configuration.GetConnectionString(nameof(MainDbContext));

            return new SqlConnection(connStr);
        }
    }
}
