
using Conta.Domain.Dto;

namespace Conta.Domain.Contas
{
    public interface IRepConta
    {
        Task<ContaCorrente?> ObterLoginAsync(LoginDto dto);
        Task<int> TaskObterUltimoNumeroAsync();
        Task AdicionarAsync(ContaCorrente conta);
    }
}
