using Conta.Domain.Contas;
using Conta.Domain.Dto;

namespace Conta.Application
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
    }
}
