using Conta.Domain.Commom;
using MediatR;


namespace Conta.Application.Contas.Queries
{
    public record ObterContaQuery(Guid idConta) : IRequest<Resultado>;
}
