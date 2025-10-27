namespace Conta.Domain.Movimentos.Dto
{
    public class MovimentoDto
    {
        public Guid? IdConta { get; set; }
        public EnumTipoMovto TipoMovimento { get; set; }
        public decimal Valor { get; set; }
    }
}
