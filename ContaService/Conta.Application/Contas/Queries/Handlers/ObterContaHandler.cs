using Conta.Domain.Commom;
using Conta.Domain.Contas;
using MediatR;

namespace Conta.Application.Contas.Queries.Handlers
{
    public class ObterContaHandler : IRequestHandler<ObterContaQuery, Resultado>
    {
        private readonly IRepConta _repConta;

        public ObterContaHandler(IRepConta repConta)
        {
            _repConta = repConta;
        }

        public async Task<Resultado> Handle(ObterContaQuery request, CancellationToken cancellationToken)
        {
            var conta = await _repConta.ObterPorIdAsync(request.idConta);

            if (conta == null)
                return Resultado.Falha("INVALID_ACCOUNT", "Conta não encontrada.");

            return Resultado.Ok(new
            {
                IdContaCorrente = conta.Id,
                NumeroConta = conta.Numero,
                Titular = conta.Cpf,
                Ativo = conta.Ativo
            });
        }
    }
}
