using Conta.Application.Contas.Queries;
using Conta.Domain.Commom;
using Conta.Domain.Contas;
using MediatR;


namespace Conta.Application.Contas.Commands.Handlers
{

    public class CriarContaHandler : IRequestHandler<CriarContaCommand, Resultado>
    {
        private readonly IRepConta _repConta;

        public CriarContaHandler(IRepConta repConta)
        {
            _repConta = repConta;
        }

        public async Task<Resultado> Handle(CriarContaCommand request, CancellationToken cancellationToken)
        {
            var ultimoNumero = await _repConta.TaskObterUltimoNumeroAsync();

            var conta = new ContaCorrente(ultimoNumero, request.Dto.Cpf, request.Dto.Senha);
            if (!ValidadorCfp.Validar(conta.Cpf))
                return Resultado.Falha("INVALID_DOCUMENT", "O CPF informado é inválido.");

            await _repConta.AdicionarAsync(conta);

            return Resultado.Ok(conta.Numero);
        }
    }
}
