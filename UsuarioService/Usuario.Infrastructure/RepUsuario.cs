using Dapper;
using Usuario.Domain.Usuarios;

namespace Usuario.Infrastructure
{
    public class RepUsuario : IRepUsuario
    {
        private readonly DapperContext _context;

        public RepUsuario(DapperContext context)
        {
            _context = context;
        }

        public async Task<User?> ObterPorIdAsync(Guid id)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT Id, Nome, Cpf FROM Usuario WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
        }
        public async Task<User?> ObterPorCpfAsync(string cpf)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT Id, Nome, Cpf, Senha FROM Usuario WHERE Cpf = @cpf";
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Cpf = cpf });
        }

        public async Task<IEnumerable<User>> ListarAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT Id, Nome, Cpf, Senha FROM Usuario";
            return await connection.QueryAsync<User>(sql);
        }

        public async Task AdicionarAsync(User usuario)
        {
            using var connection = _context.CreateConnection();
            var sql = "INSERT INTO Usuario (Id, Nome, Cpf, Senha) VALUES (@Id, @Nome, @Cpf, @Senha)";
            await connection.ExecuteAsync(sql, usuario);
        }

    }

}
