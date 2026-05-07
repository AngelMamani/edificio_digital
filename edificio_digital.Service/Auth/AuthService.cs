using edificio_digital.Entity.Auth;
using edificio_digital.Models.Auth;

namespace edificio_digital.Service.Auth;

public class AuthService(IAuthRepository authRepository) : IAuthService
{
    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        var normalizedEmail = request.Email.Trim();
        var normalizedPassword = request.Contrasena.Trim();
        var user = await authRepository.GetByEmailAsync(normalizedEmail);

        if (user is null || user.Contrasena != normalizedPassword)
        {
            return new LoginResponseDto
            {
                IsSuccess = false,
                Message = "Usuario o contraseña incorrectos."
            };
        }

        return new LoginResponseDto
        {
            IsSuccess = true,
            Message = $"BIENVENIDO {user.NombreCompleto} ESTAS CORRECTAMENTE CONECTADO A LA BASE DE DATOS",
            NombreCompleto = user.NombreCompleto,
            Email = user.Email,
            Rol = user.TipoUsuario ?? "Usuario",
            Token = $"token-demo-{Guid.NewGuid():N}"
        };
    }
}
