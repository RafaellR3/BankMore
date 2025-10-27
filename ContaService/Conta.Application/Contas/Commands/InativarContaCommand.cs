using Conta.Domain.Commom;
using MediatR;

namespace Conta.Application.Contas.Queries
{
    public record InativarContaCommand(int NumeroConta, string Senha) : IRequest<Resultado>;
}

