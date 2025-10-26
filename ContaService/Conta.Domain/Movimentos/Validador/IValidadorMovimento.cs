using Conta.Domain.Contas;
using Conta.Infrastructure.Resultados;

namespace Conta.Domain.Movimentos.Validador
{
    public interface IValidadorMovimento
    {
        Task<Resultado> ValidarAsync(Movimento movimento, ContaCorrente conta, string cpfUsuarioLogado);
    }
}
