
using Conta.Domain.Dto;

namespace Conta.Domain.Contas
{
    public interface IRepConta
    {
        Task<ContaCorrente?> ObterLoginAsync(LoginDto dto);
        Task<int> TaskObterUltimoNumeroAsync();
        Task AdicionarAsync(ContaCorrente conta);
        Task<ContaCorrente?> ObterPorNumeroAsync(int numero);
        Task AtualizarAsync(ContaCorrente conta);
    }
}
