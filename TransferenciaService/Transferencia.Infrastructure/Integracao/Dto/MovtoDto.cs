namespace Transferencia.Infrastructure.Integracao.Dto
{
    public class MovtoDto
    {
        public Guid IdConta { get; set; }
        public EnumTipoMovto TipoMovimento { get; set; }
        public decimal Valor { get; set; }
    }

    public enum EnumTipoMovto
    {
        Credito = 0,
        Debito = 1
    }
}
