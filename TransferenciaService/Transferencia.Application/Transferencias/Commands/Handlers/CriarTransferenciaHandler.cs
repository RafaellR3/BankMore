using MediatR;
using System.Text.Json;
using Transferencia.Domain.Idempotencias;
using Transferencia.Domain.Resultados;
using Transferencia.Domain.Transferencias;
using Transferencia.Infrastructure.Integracao;

namespace Transferencia.Application.Transferencias.Commands.Handlers
{

    public class CriarTransferenciaHandler : IRequestHandler<CriarTransferenciaCommand, Resultado>
    {
        private readonly IContaClient _contaClient;
        private readonly IRepTransferencia _repTransferencia;
        private readonly IRepIdempotencia _repIdempotencia;

        public CriarTransferenciaHandler(IContaClient contaClient,
                                         IRepTransferencia repTransferencia,
                                         IRepIdempotencia repIdempotencia)
        {
            _contaClient = contaClient;
            _repTransferencia = repTransferencia;
            _repIdempotencia = repIdempotencia;
        }

        public async Task<Resultado> Handle(CriarTransferenciaCommand request, CancellationToken cancellationToken)
        {
            var existente = await _repIdempotencia.ObterAsync(request.IdempotencyKey);
            if (existente != null)
                return Resultado.Ok(existente.Resultado);

            var origem = await _contaClient.ObterContaAsync(request.ContaOrigem);
            var destino = await _contaClient.ObterContaAsync(request.ContaDestino);

            if (!origem.Ativo)
                return Resultado.Falha("INACTIVE_SOURCE", "Conta de origem inativa.");
            if (!destino.Ativo)
                return Resultado.Falha("INACTIVE_TARGET", "Conta de destino inativa.");

            // 3. Validar saldo via API
            var saldoOrigem = await _contaClient.ObterSaldoAsync(request.ContaOrigem);
            if (saldoOrigem == null || saldoOrigem < request.Valor)
                return Resultado.Falha("INSUFFICIENT_FUNDS", "Saldo insuficiente.");

            var transferencia = new Transfer(
                origem.IdContaCorrente, destino.IdContaCorrente, request.Valor);

            await _repTransferencia.SalvarAsync(transferencia);

            var resultadoJson = JsonSerializer.Serialize(new
            {
                request.ContaOrigem,
                request.ContaDestino,
                request.Valor
            });


            await _repIdempotencia.SalvarAsync(
                request.IdempotencyKey,
                JsonSerializer.Serialize(request),
                resultadoJson
            );

            return Resultado.Ok(new
            {
                transferencia.Id,
                transferencia.Data,
                transferencia.Valor,
                ContaOrigem = origem.NumeroConta,
                ContaDestino = destino.NumeroConta
            });
        }
    }
}
