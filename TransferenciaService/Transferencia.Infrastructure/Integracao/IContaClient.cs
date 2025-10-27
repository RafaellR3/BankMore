using Transferencia.Domain.Resultados;
using Transferencia.Infrastructure.Integracao.Dto;

namespace Transferencia.Infrastructure.Integracao
{
    public interface IContaClient
    {
        Task<ContaDto?> ObterContaAsync(Guid idConta);
        Task<decimal?> ObterSaldoAsync(Guid idConta);
        Task<Resultado> SalvarMovto(MovtoDto dto);
        Task<Resultado> EstornarMovto(Guid idMovto);
    }

}
