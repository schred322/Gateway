using Microsoft.Data.SqlClient;

namespace Gateway.Interfaces
{
    public interface IDbServiceProvider
    {
        SqlConnection GetSqlConnection(string connectionString);
    }
}