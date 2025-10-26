namespace Usuario.Domain.Usuarios
{
    public class User
    {
        public User()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string Nome { get; private set; }
        public string Cpf { get; private set; }
        public string Senha { get; private set; }

        public User(string nome, string cpf, string senha)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Cpf = cpf;
            Senha = BCrypt.Net.BCrypt.HashPassword(senha);
        }

        public bool VerificarSenha(string senhaDigitada)
        {
            return BCrypt.Net.BCrypt.Verify(senhaDigitada, Senha);
        }

    }
}
