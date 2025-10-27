using Conta.Domain.Commom;
using Conta.Domain.Contas;

namespace Conta.Domain.Movimentos.Validador
{
    public interface IValidadorMovimento
    {
        Task<Resultado> ValidarAsync(Movimento movimento, ContaCorrente conta, string cpfUsuarioLogado);
    }
}
