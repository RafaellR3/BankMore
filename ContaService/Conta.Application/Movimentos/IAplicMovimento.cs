using Conta.Domain.Movimentos.Dto;
using Conta.Infrastructure;
using Conta.Infrastructure.Resultados;

namespace Conta.Application.Movimentos
{
    public interface IAplicMovimento
    {
        Task<Resultado> Criar(MovimentoDto dto, string idempotencyKey, DadosAmbiente dadosAmbiente);
    }
}
