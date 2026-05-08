using edificio_digital.Models.Domain.Auth;

namespace edificio_digital.Service.Auth;

public interface IJwtTokenService
{
    AccessTokenResult CreateAccessToken(UserCredential user);
    string CreateRefreshTokenValue();
    string HashRefreshToken(string token);
}

public record AccessTokenResult(string Token, DateTime ExpiresAt);
