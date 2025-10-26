using Usuario.Application.Dto;
using Usuario.Application.View;

namespace Usuario.Application
{
    public interface IAplicUser
    {
        Task<UserView> CriarUsuario(UserDto dto);
    }
}
