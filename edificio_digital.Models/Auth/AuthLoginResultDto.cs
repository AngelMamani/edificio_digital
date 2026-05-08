namespace edificio_digital.Models.Auth;

public class AuthLoginResultDto
{
    public LoginResponseDto Response { get; set; } = new();
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiresAt { get; set; }
}
