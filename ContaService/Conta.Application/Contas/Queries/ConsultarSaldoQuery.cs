using Conta.Domain.Commom;
using MediatR;


namespace Conta.Application.Contas.Queries
{

    public record ConsultarSaldoQuery(Guid idConta) : IRequest<Resultado>;

}
