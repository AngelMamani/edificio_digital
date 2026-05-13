using edificio_digital.Models.Models.Usuarios;

namespace edificio_digital.Models.Domain.Usuarios
{
    public interface IUsuario
    {
        Task<List<UserDto>> IgetUsers();
    }
}
