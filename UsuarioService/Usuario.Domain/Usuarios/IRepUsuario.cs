
namespace Usuario.Domain.Usuarios
{
    public interface IRepUsuario
    {
        Task<User?> ObterPorIdAsync(Guid id);
        Task<User?> ObterPorCpfAsync(string cpf);
        Task<IEnumerable<User>> ListarAsync();
        Task AdicionarAsync(User usuario);
    }

}

