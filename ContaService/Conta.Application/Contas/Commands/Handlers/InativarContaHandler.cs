using Conta.Application.Contas.Queries;
using Conta.Domain.Commom;
using Conta.Domain.Contas;
using MediatR;

public class InativarContaHandler : IRequestHandler<InativarContaCommand, Resultado>
{
    private readonly IRepConta _repConta;

    public InativarContaHandler(IRepConta repConta)
    {
        _repConta = repConta;
    }

    public async Task<Resultado> Handle(InativarContaCommand request, CancellationToken cancellationToken)
    {
        var conta = await _repConta.ObterPorNumeroAsync(request.NumeroConta);

        if (conta == null)
            return Resultado.Falha("INVALID_ACCOUNT", "Conta não encontrada.");

        if (!conta.VerificarSenha(request.Senha))
            return Resultado.Falha("INVALID_PASSWORD", "Senha inválida.");

        if (!conta.Ativo)
            return Resultado.Falha("INACTIVE_ACCOUNT", "Conta já está inativa.");

        conta.Inativar();
        await _repConta.AtualizarAsync(conta);

        return Resultado.Ok(new { NumeroConta = conta.Numero, Status = "Inativada" });
    }
}
