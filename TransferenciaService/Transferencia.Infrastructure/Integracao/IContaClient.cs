using Transferencia.Infrastructure.Integracao.Dto;

namespace Transferencia.Infrastructure.Integracao
{
    public interface IContaClient
    {
        Task<ContaDto?> ObterContaAsync(int numeroConta);
        Task<decimal?> ObterSaldoAsync(int numeroConta);
    }

}
