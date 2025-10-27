namespace Transferencia.Domain.Transferencias;

public class Transfer
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid IdContaCorrenteOrigem { get; private set; }
    public Guid IdContaCorrenteDestino { get; private set; }
    public decimal Valor { get; private set; }
    public DateTime DataMovimentacao { get; private set; }

    public Transfer(Guid origem, Guid destino, decimal valor)
    {

        IdContaCorrenteOrigem = origem;
        IdContaCorrenteDestino = destino;
        Valor = valor;
        DataMovimentacao = DateTime.Now;
    }
}