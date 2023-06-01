
using Microsoft.Data.SqlClient;

namespace NetTemplate.Shared.Infrastructure.Background.Utils
{
    public static class HangfireHelper
    {
        public static async Task InitHangfireDatabase(string masterConnStr, string HangfireDbName, CancellationToken cancellationToken = default)
        {
            using (SqlConnection connection = new SqlConnection(masterConnStr))
            {
                connection.Open();

                string sqlCmd = string.Format(
                    @"
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'{0}') 
CREATE DATABASE [{0}];", HangfireDbName);

                using (SqlCommand command = new SqlCommand(sqlCmd, connection))
                {
                    await command.ExecuteNonQueryAsync(cancellationToken);
                }
            }
        }
    }
}
