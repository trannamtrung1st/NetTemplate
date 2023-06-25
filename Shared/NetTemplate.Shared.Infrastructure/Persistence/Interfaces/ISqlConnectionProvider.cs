using Microsoft.Data.SqlClient;

namespace NetTemplate.Shared.Infrastructure.Persistence.Interfaces
{
    public interface ISqlConnectionProvider
    {
        SqlConnection CreateConnection(string connectionString = default);
    }
}
