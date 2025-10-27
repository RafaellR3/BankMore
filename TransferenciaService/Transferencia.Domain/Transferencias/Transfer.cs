namespace Transferencia.Domain.Transferencias;

public class Transfer
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid IdContaCorenteOrigem { get; private set; }
    public Guid IdContaCorrenteDestino { get; private set; }
    public decimal Valor { get; private set; }
    public DateTime Data { get; private set; }

    public Transfer(Guid origem, Guid destino, decimal valor)
    {

        IdContaCorenteOrigem = origem;
        IdContaCorrenteDestino = destino;
        Valor = valor;
        Data = DateTime.Now;
    }
}