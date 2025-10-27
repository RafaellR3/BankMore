using Conta.Domain.Commom;
using Conta.Domain.Dto;
using MediatR;

namespace Conta.Application.Contas.Queries
{
    public record CriarContaCommand(ContaDto Dto) : IRequest<Resultado>;
}