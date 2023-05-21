using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NetTemplate.Blog.Infrastructure.Persistence.Interfaces;
using NetTemplate.Common.DependencyInjection;

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
