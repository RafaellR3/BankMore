using Transferencia.Infrastructure.Resultados;

namespace Transferencia.Application.Transferencias
{
    public interface IAplicTransferencia
    {
        Task<Resultado> EfetuarAsync(string origem, string destino, decimal valor, string token);
    }

}
