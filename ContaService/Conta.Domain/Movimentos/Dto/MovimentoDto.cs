namespace Conta.Domain.Movimentos.Dto
{
    public class MovimentoDto
    {
        public int? NumeroConta { get; set; }
        public EnumTipoMovto TipoMovimento { get; set; }
        public decimal Valor { get; set; }
    }
}
