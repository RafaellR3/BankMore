using Conta.Domain.Commom;
using Conta.Domain.Contas;

namespace Conta.Domain.Movimentos.Validador
{
    public class ValidadorMovimento : IValidadorMovimento
    {
        public async Task<Resultado> ValidarAsync(Movimento movimento, ContaCorrente conta, string cpfUsuarioLogado)
        {
            if (!conta.Ativo)
                return Resultado.Falha("CONTA_INATIVA", "A conta informada está inativa.");

            if (movimento.Valor <= 0)
                return Resultado.Falha("INVALID_VALUE", "O valor deve ser positivo.");

            if (movimento.TipoMovimento != EnumTipoMovto.Credito && movimento.TipoMovimento != EnumTipoMovto.Debito)
                return Resultado.Falha("INVALID_TYPE", "O tipo de movimento deve ser 'Crédito' ou 'Débito'.");

            if (conta.Cpf != cpfUsuarioLogado && movimento.TipoMovimento != EnumTipoMovto.Credito)
                return Resultado.Falha("INVALID_TYPE", "Somente crédito é permitido em contas diferentes do usuário logado.");

            return Resultado.Ok();
        }
    }
}
