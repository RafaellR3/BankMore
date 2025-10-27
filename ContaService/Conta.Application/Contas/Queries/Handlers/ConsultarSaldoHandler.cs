using Conta.Domain.Commom;
using Conta.Domain.Contas;
using MediatR;

namespace Conta.Application.Contas.Queries.Handlers
{
    public class ConsultarSaldoHandler : IRequestHandler<ConsultarSaldoQuery, Resultado>
    {
        private readonly IRepConta _repConta;

        public ConsultarSaldoHandler(IRepConta repConta)
        {
            _repConta = repConta;
        }

        public async Task<Resultado> Handle(ConsultarSaldoQuery request, CancellationToken cancellationToken)
        {
            var conta = await _repConta.ObterPorIdAsync(request.idConta);

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
