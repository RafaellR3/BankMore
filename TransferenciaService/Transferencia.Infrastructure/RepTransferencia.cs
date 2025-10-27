using Dapper;
using System.Data;
using Transferencia.Domain.Transferencias;
using Transferencia.Infrastructure;

namespace Conta.Infrastructure.Transferencias
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
                        VALUES (@Id, @IdContaCorenteOrigem, @IdContaCorrenteDestino, @Valor, @Data)";

            await conn.ExecuteAsync(sql, new
            {
                Id = transferencia.Id,
                Origem = transferencia.IdContaCorenteOrigem,
                Destino = transferencia.IdContaCorrenteDestino,
                Valor = transferencia.Valor,
                DataMovimentacao = transferencia.Data
            });
        }
    }
}