using edificio_digital.Models.Auth;

namespace edificio_digital.Service.Auth;

public interface IAuthService
{
    Task<AuthLoginResultDto> LoginAsync(LoginRequestDto request);
    Task<RefreshTokenResultDto> RefreshAsync(string refreshToken);
    Task LogoutAsync(string refreshToken);
}
