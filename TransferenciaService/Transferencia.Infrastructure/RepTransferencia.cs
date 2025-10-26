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
                        (Id, NumeroContaOrigem, NumeroContaDestino, Valor, DataMovimentacao) 
                        VALUES (@Id, @Origem, @Destino, @Valor, @Data)";

            await conn.ExecuteAsync(sql, new
            {
                Id = transferencia.Id,
                Origem = transferencia.NumeroContaOrigem,
                Destino = transferencia.NumeroContaDestino,
                Valor = transferencia.Valor,
                DataMovimentacao = transferencia.Data
            });
        }
    }
}