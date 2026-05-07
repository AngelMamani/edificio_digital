namespace edificio_digital.Entity.Auth;

public class InMemoryAuthRepository : IAuthRepository
{
    private static readonly List<UserCredentialEntity> Users =
    [
        new()
        {
            Email = "admin@edificiodigital.com",
            Contrasena = "123456",
            TipoUsuario = "Administrador",
            NombreCompleto = "Usuario Administrador (ejemplo)"
        },
        new()
        {
            Email = "docente@edificiodigital.com",
            Contrasena = "123456",
            TipoUsuario = "Docente",
            NombreCompleto = "Usuario Docente (ejemplo)"
        },
        new()
        {
            Email = "alumno@edificiodigital.com",
            Contrasena = "123456",
            TipoUsuario = "Alumno",
            NombreCompleto = "Usuario Alumno (ejemplo)"
        }
    ];

    public Task<UserCredentialEntity?> GetByEmailAsync(string email)
    {
        var user = Users.FirstOrDefault(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(user);
    }
}
