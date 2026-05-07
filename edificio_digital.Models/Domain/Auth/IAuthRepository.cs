namespace edificio_digital.Models.Domain.Auth;

public interface IAuthRepository
{
    Task<UserCredential?> GetByEmailAsync(string email);
}
