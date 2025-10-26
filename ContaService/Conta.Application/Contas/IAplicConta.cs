using Conta.Domain.Contas;
using Conta.Domain.Dto;

namespace Conta.Application.Contas
{
    public interface IAplicConta
    {
        Task<int> Criar(ContaDto dto);
        Task Inativar(ContaCorrente conta);
    }
}
