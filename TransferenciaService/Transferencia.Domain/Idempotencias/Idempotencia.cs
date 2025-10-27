namespace Transferencia.Domain.Idempotencias
{
    public class Idempotencia
    {
        public string Chave_Idempotencia { get; set; }
        public string Requisicao { get; set; }
        public string Resultado { get; set; }
    }
}
