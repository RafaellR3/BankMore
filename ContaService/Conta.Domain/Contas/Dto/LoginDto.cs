using System.ComponentModel;

namespace Conta.Domain.Dto
{
    public class LoginDto
    {
        public string Login { get; set; }
        public string Senha { get; set; }
        public EnumTipoLogin TipoLogin { get; set; }
    }

    public enum EnumTipoLogin
    {
        [Description("Cpf")]
        Cpf =0,
        [Description("Número conta")]
        NumeroConta = 1
    }
}
