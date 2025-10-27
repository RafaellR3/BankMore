using MediatR;
using Transferencia.Domain.Idempotencias;
using Transferencia.Domain.Resultados;
using Transferencia.Domain.Transferencias;
using Transferencia.Infrastructure.Integracao;
using Transferencia.Infrastructure.Integracao.Dto;
using Newtonsoft.Json;


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

            if (origem == null)
                return Resultado.Falha("INVALID_ACCOUNT", "Conta origem não encontrada.");
            if (destino == null)
                return Resultado.Falha("INVALID_ACCOUNT", "Conta destino não encontrada.");

            if (!origem.Ativo)
                return Resultado.Falha("INACTIVE_SOURCE", "Conta de origem inativa.");
            if (!destino.Ativo)
                return Resultado.Falha("INACTIVE_TARGET", "Conta de destino inativa.");

            var saldoOrigem = await _contaClient.ObterSaldoAsync(request.ContaOrigem);
            if (saldoOrigem == null || saldoOrigem < request.Valor)
                return Resultado.Falha("INSUFFICIENT_FUNDS", "Saldo insuficiente.");

            var transferencia = new Transfer(
                origem.IdContaCorrente, destino.IdContaCorrente, request.Valor);

            var resutMovtos = await GerarMovtos(transferencia);
            if (!resutMovtos.Sucesso)
                return resutMovtos;

            await _repTransferencia.SalvarAsync(transferencia);

            await _repIdempotencia.SalvarAsync(request.IdempotencyKey,
                                               JsonConvert.SerializeObject(request),
                                               JsonConvert.SerializeObject(new
                                               {
                                                   request.ContaOrigem,
                                                   request.ContaDestino,
                                                   request.Valor
                                               })
            );

            return Resultado.Ok();
        }

        private async Task<Resultado> GerarMovtos(Transfer transfer)
        {
            var resultadoOrigem = await _contaClient.SalvarMovto(new MovtoDto
            {
                IdConta = transfer.IdContaCorrenteOrigem,
                TipoMovimento = EnumTipoMovto.Debito,
                Valor = transfer.Valor
            });

            if (!resultadoOrigem.Sucesso)
                return resultadoOrigem;

            var resultaDestino = await _contaClient.SalvarMovto(new MovtoDto
            {
                IdConta = transfer.IdContaCorrenteDestino,
                TipoMovimento = EnumTipoMovto.Credito,
                Valor = transfer.Valor
            });

            if (!resultaDestino.Sucesso)
            {
                var dto = JsonConvert.DeserializeObject<RetornoMovtoDto>(resultadoOrigem.Dados.ToString());
                await _contaClient.EstornarMovto(dto.Id);
            }

            return Resultado.Ok();
        }
    }
}
