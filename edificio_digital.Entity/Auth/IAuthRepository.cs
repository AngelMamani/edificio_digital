namespace edificio_digital.Entity.Auth;

public interface IAuthRepository
{
    Task<UserCredentialEntity?> GetByEmailAsync(string email);
}
