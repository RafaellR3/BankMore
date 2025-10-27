using Conta.Domain.Commom;
using MediatR;


namespace Conta.Application.Contas.Queries
{
    public record ObterContaQuery(int NumeroConta) : IRequest<Resultado>;
}
