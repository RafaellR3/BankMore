namespace Transferencia.Infrastructure.Integracao.Dto
{
    public class RetornoMovtoDto
    {
        public Guid Id { get; set; }
        public EnumTipoMovto TipoMovimento { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataMovimentacao { get; set; }
    }
}
