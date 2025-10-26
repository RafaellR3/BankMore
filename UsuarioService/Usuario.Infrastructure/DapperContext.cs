using System.Data;
using MySql.Data.MySqlClient;

namespace Usuario.Infrastructure;

public class DapperContext
{
    private readonly string _connectionString;

    public DapperContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
        => new MySqlConnection(_connectionString);
}