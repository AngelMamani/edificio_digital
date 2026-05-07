using edificio_digital.Models.Domain.Auth;

namespace edificio_digital.Entity.Auth;

public class BcryptPasswordHasher : IPasswordHasher
{
    private const int WorkFactor = 12;

    public string Hash(string plain) =>
        BCrypt.Net.BCrypt.HashPassword(plain, WorkFactor);

    public bool Verify(string plain, string hash)
    {
        if (string.IsNullOrEmpty(hash)) return false;
        try
        {
            return BCrypt.Net.BCrypt.Verify(plain, hash);
        }
        catch (BCrypt.Net.SaltParseException)
        {
            return false;
        }
    }
}
