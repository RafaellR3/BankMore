namespace Conta.Domain.Movimentos
{
    public interface IRepMovimento
    {
        Task AdicionarAsync(Movimento movimento);
        Task<Movimento?> ListarPorContaAsync(string idContaCorrente);
    }
}
