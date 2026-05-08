namespace edificio_digital.Models.Domain.Auth;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken token);
    Task<RefreshToken?> FindByHashAsync(string tokenHash);
    Task UpdateAsync(RefreshToken token);
    Task RevokeAllForUserAsync(Guid userId);
}
