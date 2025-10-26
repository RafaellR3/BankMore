using Usuario.Domain.Usuarios;

namespace Usuario.Application.View
{
    public class UserView
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Senha { get; set; }

        public static List<UserView> Novo(List<User> users)
        {
            return users.Select(user => new UserView
            {
                Id = user.Id,
                Nome = user.Nome,
                Cpf = user.Cpf,
                Senha = user.Senha
            }).ToList();
        }

        public static UserView Novo(User user)
        {
             return new UserView
            {
                Id = user.Id,
                Nome = user.Nome,
                Cpf = user.Cpf,
                Senha = user.Senha
            };
        }
    }
}
