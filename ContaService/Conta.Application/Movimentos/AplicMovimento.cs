using Conta.Domain.Contas;
using Conta.Domain.Idempotencias;
using Conta.Domain.Movimentos;
using Conta.Domain.Movimentos.Dto;
using Conta.Domain.Movimentos.Validador;
using Conta.Infrastructure;
using Conta.Infrastructure.Resultados;
using System.Text.Json;

namespace Conta.Application.Movimentos
{
    public class AplicMovimento : IAplicMovimento
    {
        private readonly IRepMovimento _repMovimento;
        private readonly IRepConta _repConta;
        private readonly IRepIdempotencia _repIdempotencia;
        private readonly IValidadorMovimento _validadorMovimento;
        public AplicMovimento(IRepMovimento repMovimento,
                              IRepConta repConta,
                              IRepIdempotencia repIdempotencia,
                              IValidadorMovimento validadorMovimento)
        {
            _repMovimento = repMovimento;
            _repConta = repConta;
            _repIdempotencia = repIdempotencia;
            _validadorMovimento = validadorMovimento;
        }
        public async Task<Resultado> Criar(MovimentoDto dto, string idempotencyKey, DadosAmbiente dadosAmbiente)
        {
            var conta = await _repConta.ObterPorNumeroAsync(dto.NumeroConta.Value);
            if (conta == null)
                return Resultado.Falha("CONTA_INVÁLIDA", "A conta informada não está cadastrada.");

            var movimento = new Movimento
            {
                IdContaCorrente = conta.Id,
                DataMovimentacao = DateTime.Now,
                TipoMovimento = dto.TipoMovimento,
                Valor = dto.Valor
            };
            var resultValidacao = await _validadorMovimento.ValidarAsync(movimento, conta, dadosAmbiente.CpfUsuarioLogado);

            if (!resultValidacao.Sucesso)
                return resultValidacao;

            await _repMovimento.AdicionarAsync(movimento);

            var resultado = JsonSerializer.Serialize(movimento);

            await _repIdempotencia.SalvarAsync(
                idempotencyKey,
                JsonSerializer.Serialize(dto),
                resultado
            );

            return Resultado.Ok();
        }

    }
}
