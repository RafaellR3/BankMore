using MySql.Data.MySqlClient;
using System.Data;

namespace Conta.Infrastructure
{
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
}
