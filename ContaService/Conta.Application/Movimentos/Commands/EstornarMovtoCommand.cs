using Conta.Domain.Commom;
using MediatR;

namespace Conta.Application.Movimentos.Commands
{
    public record EstornarMovtoCommand(Guid idMovto)
        : IRequest<Resultado>;
}
