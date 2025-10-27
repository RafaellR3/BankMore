using MediatR;
using Transferencia.Domain.Resultados;

namespace Transferencia.Application.Transferencias.Commands
{
  
    public record CriarTransferenciaCommand(
        int ContaOrigem,
        int ContaDestino,
        decimal Valor,
        string IdempotencyKey
    ) : IRequest<Resultado>;
}
