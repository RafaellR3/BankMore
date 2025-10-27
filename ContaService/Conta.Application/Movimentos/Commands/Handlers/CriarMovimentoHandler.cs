using Conta.Domain.Commom;
using Conta.Domain.Contas;
using Conta.Domain.Idempotencias;
using Conta.Domain.Movimentos;
using Conta.Domain.Movimentos.Validador;
using MediatR;
using System.Text.Json;

namespace Conta.Application.Movimentos.Commands.Handlers
{
    public class CriarMovimentoHandler : IRequestHandler<CriarMovimentoCommand, Resultado>
    {
        private readonly IRepMovimento _repMovimento;
        private readonly IRepConta _repConta;
        private readonly IRepIdempotencia _repIdempotencia;
        private readonly IValidadorMovimento _validadorMovimento;

        public CriarMovimentoHandler( IRepMovimento repMovimento,
                                      IRepConta repConta,
                                      IRepIdempotencia repIdempotencia,
                                      IValidadorMovimento validadorMovimento)
        {
            _repMovimento = repMovimento;
            _repConta = repConta;
            _repIdempotencia = repIdempotencia;
            _validadorMovimento = validadorMovimento;
        }

        public async Task<Resultado> Handle(CriarMovimentoCommand request, CancellationToken cancellationToken)
        {

            var existente = await _repIdempotencia.ObterAsync(request.IdempotencyKey);
            if (existente != null)
                return Resultado.Ok(JsonSerializer.Deserialize<object>(existente.Resultado));           

            var conta = await _repConta.ObterPorIdAsync(request.Dto.IdConta.Value);
            if (conta == null)
                return Resultado.Falha("INVALID_ACCOUNT", "Conta não encontrada.");

            var movimento = new Movimento
            {
                IdContaCorrente = conta.Id,
                DataMovimentacao = DateTime.Now,
                TipoMovimento = request.Dto.TipoMovimento,
                Valor = request.Dto.Valor
            };

            var validacao = await _validadorMovimento.ValidarAsync(movimento, conta, request.DadosAmbiente.CpfUsuarioLogado);
            if (!validacao.Sucesso)
                return validacao;

            await _repMovimento.AdicionarAsync(movimento);

            var resultadoJson = JsonSerializer.Serialize(new
            {
                movimento.Id,
                movimento.TipoMovimento,
                movimento.Valor,
                movimento.DataMovimentacao
            });

            await _repIdempotencia.SalvarAsync(
                request.IdempotencyKey,
                JsonSerializer.Serialize(request.Dto),
                resultadoJson
            );

            return Resultado.Ok(new
            {
                movimento.Id,
                movimento.TipoMovimento,
                movimento.Valor,
                movimento.DataMovimentacao
            });
        }
    }
}
