using edificio_digital.Models.Auth;

namespace edificio_digital.Service.Auth;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
}
