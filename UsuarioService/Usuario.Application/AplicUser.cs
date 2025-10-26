using Usuario.Application.Dto;
using Usuario.Application.View;
using Usuario.Domain.Usuarios;

namespace Usuario.Application
{
    public class AplicUser: IAplicUser
    {
        private readonly IRepUsuario _repUser;
        public AplicUser(IRepUsuario repUser)
        {
            _repUser = repUser;
        }

        public async Task<UserView> CriarUsuario(UserDto dto)
        {
            var usuario = new User(dto.Nome, dto.Cpf, dto.Senha);

            //todo: validar usuario 

            await _repUser.AdicionarAsync(usuario);
            return UserView.Novo(usuario);
        }
    }
}
