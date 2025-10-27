using MediatR;
using Transferencia.Domain.Resultados;

namespace Transferencia.Application.Transferencias.Commands
{
  
    public record CriarTransferenciaCommand(
        Guid ContaOrigem,
        Guid ContaDestino,
        decimal Valor,
        string IdempotencyKey
    ) : IRequest<Resultado>;
}
