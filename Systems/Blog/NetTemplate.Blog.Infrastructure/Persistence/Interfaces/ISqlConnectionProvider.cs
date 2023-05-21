using Microsoft.Data.SqlClient;

namespace NetTemplate.Blog.Infrastructure.Persistence.Interfaces
{
    public interface ISqlConnectionProvider
    {
        SqlConnection CreateConnection();
    }
}
