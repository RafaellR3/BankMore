using Dapper;
using Transferencia.Domain.Transferencias;

namespace Transferencia.Infrastructure
{

    public class RepTransferencia : IRepTransferencia
    {
        private readonly DapperContext _context;

        public RepTransferencia(DapperContext context)
        {
            _context = context;
        }

        public async Task SalvarAsync(Transfer transferencia)
        {
            using var conn = _context.CreateConnection();
            var sql = @"INSERT INTO transferencia 
                        (Id, IdContaCorrenteOrigem, IdContaCorrenteDestino, Valor, DataMovimentacao) 
                        VALUES (@Id, @IdContaCorrenteOrigem, @IdContaCorrenteDestino, @Valor, @DataMovimentacao)";

            await conn.ExecuteAsync(sql, new
            {
                transferencia.Id,
                transferencia.IdContaCorrenteOrigem,
                transferencia.IdContaCorrenteDestino,
                transferencia.Valor,
                transferencia.DataMovimentacao
            });
        }
    }
}