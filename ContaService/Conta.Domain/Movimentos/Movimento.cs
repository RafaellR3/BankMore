namespace Conta.Domain.Movimentos
{
    public class Movimento
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid IdContaCorrente { get; set; }
        public DateTime DataMovimentacao { get; set; }
        public EnumTipoMovto TipoMovimento { get; set; } 
        public decimal Valor { get; set; }
    }

    public enum EnumTipoMovto
    {
        Credito = 0,
        Debito = 1
    }
}
