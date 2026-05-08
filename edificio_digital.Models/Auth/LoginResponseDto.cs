namespace edificio_digital.Models.Auth;

public class LoginResponseDto
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid? UserId { get; set; }
    public string? NombreCompleto { get; set; }
    public string? Email { get; set; }
    public string? Rol { get; set; }
    public List<string> Roles { get; set; } = [];
    public string? AccessToken { get; set; }
    public DateTime? AccessTokenExpiresAt { get; set; }
}
