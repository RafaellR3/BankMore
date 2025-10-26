namespace Conta.Domain.Idempotencias
{
    public interface IRepIdempotencia
    {
        Task<Idempotencia?> ObterAsync(string chave);
        Task SalvarAsync(string chave, string requisicao, string resultado);
    }
}
