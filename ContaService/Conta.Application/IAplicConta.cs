using Conta.Domain.Dto;

namespace Conta.Application
{
    public interface IAplicConta
    {
        Task<int> Criar(ContaDto dto);
    }
}
