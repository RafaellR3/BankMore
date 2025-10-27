namespace Transferencia.Infrastructure.Integracao.Dto
{
    public class ContaDto
    {
        public Guid IdContaCorrente { get; set; }
        public int NumeroConta { get; set;  }
        public string Titular { get; set; }
        public bool Ativo {  get; set; }
    }
}
