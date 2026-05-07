namespace edificio_digital.Models.Domain.Auth;

public interface IPasswordHasher
{
    string Hash(string plain);
    bool Verify(string plain, string hash);
}
