using Conta.Domain.Contas;
using Conta.Domain.Dto;
using Conta.Infrastructure.Resultados;

namespace Conta.Application.Contas
{
    public class AplicConta: IAplicConta
    {
        private readonly IRepConta _repConta;
        public AplicConta(IRepConta repConta)
        {
            _repConta = repConta;
        }

        public async Task<int> Criar(ContaDto dto)
        {
            var ultimoNumero = await _repConta.TaskObterUltimoNumeroAsync();
            var conta = new ContaCorrente(ultimoNumero, dto.Cpf, dto.Senha);

            await _repConta.AdicionarAsync(conta);

            return conta.Numero;
        }

        public async Task Inativar(ContaCorrente conta)
        {
            conta.Inativar();   
            await _repConta.AtualizarAsync(conta);
        }

        public async Task<Resultado> ConsultarSaldoAsync(int numeroConta)
        {
            var conta = await _repConta.ObterPorNumeroAsync(numeroConta);
            if (conta == null)
                return Resultado.Falha("INVALID_ACCOUNT", "Conta não encontrada.");

            if (!conta.Ativo)
                return Resultado.Falha("INACTIVE_ACCOUNT", "Conta está inativa.");

            var saldo = await _repConta.CalcularSaldoAsync(conta.Id);

            return Resultado.Ok(new
            {
                NumeroConta = conta.Numero,
                Titular = conta.Cpf,
                SaldoAtual = saldo,
                DataConsulta = DateTime.Now
            });
        }

    }
}
