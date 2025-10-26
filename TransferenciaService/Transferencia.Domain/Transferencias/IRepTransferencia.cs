namespace Transferencia.Domain.Transferencias
{
    public interface IRepTransferencia
    {
        Task SalvarAsync(Transfer transferencia);
    }

}
