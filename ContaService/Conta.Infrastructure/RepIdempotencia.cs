using Conta.Domain.Idempotencias;
using Dapper;

namespace Conta.Infrastructure
{
    public class RepIdempotencia : IRepIdempotencia
    {
        private readonly DapperContext _context;

        public RepIdempotencia(DapperContext context)
        {
            _context = context;
        }

        public async Task<Idempotencia?> ObterAsync(string chave)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM idempotencia WHERE chave_idempotencia = @chave";
            return await connection.QueryFirstOrDefaultAsync<Idempotencia>(sql, new { chave });
        }

        public async Task SalvarAsync(string chave, string requisicao, string resultado)
        {
            using var connection = _context.CreateConnection();
            var sql = @"INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado)
                    VALUES (@chave, @requisicao, @resultado)";
            await connection.ExecuteAsync(sql, new { chave, requisicao, resultado });
        }

    }
}
