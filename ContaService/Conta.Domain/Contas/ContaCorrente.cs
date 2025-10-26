using System.Security.Cryptography;

namespace Conta.Domain.Contas
{
    public class ContaCorrente
    {
        public ContaCorrente() { }
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Numero { get; set; }
        public string Cpf { get; set; }
        public bool Ativo { get; set; } = true;
        public string Senha { get; set; }
        public string Salt { get; set; }

        public ContaCorrente(int ultimoNumero, string cpf, string senha)
        {
            Numero = 0;
            Cpf = cpf;
            Numero = int.Parse(GeradorSequencialDeNumero(ultimoNumero));
            GerarSenha(senha);

        }
        public void GerarSenha(string senha)
        {

            byte[] salt = GerarSalt(16);

            Salt = Convert.ToBase64String(salt);
            Senha = Convert.ToBase64String(GerarHashSenha(senha, salt));

        }

        public bool VerificarSenha(string senhaDigitada)
        {
            var saltBytes = Convert.FromBase64String(Salt);
            var hashTentativa = Convert.ToBase64String(GerarHashSenha(senhaDigitada, saltBytes));
            return hashTentativa == Senha;
        }

        public byte[] GerarSalt(int tamanho)
        {
            byte[] salt = new byte[tamanho];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public byte[] GerarHashSenha(string senha, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(senha, salt, 100000, HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(32);
            }
        }

        public string GeradorSequencialDeNumero(int last)
        {
            lock (this)
            {
                last++;
                return last.ToString("D7");
            }
        }

    }
}
