using Conta.Domain.Movimentos;
using Dapper;
using System.Data;

namespace Conta.Infrastructure
{
    public class RepMovimento: IRepMovimento
    {

        private readonly DapperContext _context;
        public RepMovimento(DapperContext context)
        {
            _context = context; 
        }

        public async Task AdicionarAsync(Movimento movimento)
        {
            using var connection = _context.CreateConnection();
            var sql = @"INSERT INTO movimento 
                    ( Id, IdContaCorrente, DataMovimentacao, TipoMovimento, Valor)
                    VALUES (@Id, @IdContaCorrente, @DataMovimentacao, @TipoMovimento, @Valor)";

            await connection.ExecuteAsync(sql, movimento);
        }

        public async Task<Movimento?> ListarPorContaAsync(string idContaCorrente)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM movimento WHERE idcontacorrente = @idContaCorrente";
            return await connection.QueryFirstOrDefaultAsync<Movimento>(sql, new { idContaCorrente });
        }

        public async Task<Movimento?> ObeterPorIdAsync(Guid idMovto)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM movimento WHERE Id = @idMovto";
            return await connection.QueryFirstOrDefaultAsync<Movimento>(sql, new { idMovto });
        }
    }
}
