namespace Conta.Infrastructure.Resultados
{
    public class Resultado
    {
        public bool Sucesso { get; private set; }
        public string? TipoErro { get; private set; }
        public string? Mensagem { get; private set; }

        public static Resultado Ok() => new Resultado { Sucesso = true };
        public static Resultado Falha(string tipoErro, string mensagem) =>
            new Resultado { Sucesso = false, TipoErro = tipoErro, Mensagem = mensagem };
    }
}
