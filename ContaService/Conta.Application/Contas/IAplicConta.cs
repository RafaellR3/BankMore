using Conta.Domain.Contas;
using Conta.Domain.Dto;
using Conta.Infrastructure.Resultados;

namespace Conta.Application.Contas
{
    public interface IAplicConta
    {
        Task<int> Criar(ContaDto dto);
        Task Inativar(ContaCorrente conta);
        Task<Resultado> ConsultarSaldoAsync(int numeroConta);
    }
}
