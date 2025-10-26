namespace Transferencia.Domain.Transferencias;

public class Transfer
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string NumeroContaOrigem { get; private set; }
    public string NumeroContaDestino { get; private set; }
    public decimal Valor { get; private set; }
    public DateTime Data { get; private set; }

    public Transfer(string origem, string destino, decimal valor)
    {

        NumeroContaOrigem = origem;
        NumeroContaDestino = destino;
        Valor = valor;
        Data = DateTime.Now;
    }
}