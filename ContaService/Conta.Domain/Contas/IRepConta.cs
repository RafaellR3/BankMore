
using Conta.Domain.Dto;

namespace Conta.Domain.Contas
{
    public interface IRepConta
    {
        Task<ContaCorrente?> ObterLoginAsync(LoginDto dto);
        Task<int> TaskObterUltimoNumeroAsync();
        Task AdicionarAsync(ContaCorrente conta);
        Task<ContaCorrente?> ObterPorNumeroAsync(int numero);
        Task<ContaCorrente?> ObterPorIdAsync(Guid id);
        Task AtualizarAsync(ContaCorrente conta);
        Task<decimal> CalcularSaldoAsync(Guid IdContaCorrente);
    }
}
