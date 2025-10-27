namespace Conta.Domain.Commom
{
    public class Resultado
    {
        public bool Sucesso { get; private set; }
        public string? TipoErro { get; private set; }
        public string? Mensagem { get; private set; }
        public object? Dados { get; private set; }


        public static Resultado Ok(object? dados) => new Resultado { Sucesso = true, Dados = dados }; 
        public static Resultado Ok() => new Resultado { Sucesso = true};
        public static Resultado Falha(string tipoErro, string mensagem) =>
            new Resultado { Sucesso = false, TipoErro = tipoErro, Mensagem = mensagem };
    }
}
