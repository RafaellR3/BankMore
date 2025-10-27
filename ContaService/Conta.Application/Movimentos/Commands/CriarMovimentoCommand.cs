
using Conta.Domain.Commom;
using Conta.Domain.Movimentos.Dto;
using Conta.Infrastructure;
using MediatR;

namespace Conta.Application.Movimentos.Commands
{

    public record CriarMovimentoCommand(MovimentoDto Dto, string IdempotencyKey, DadosAmbiente DadosAmbiente)
        : IRequest<Resultado>;
}
