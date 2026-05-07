namespace edificio_digital.Models.Auth;

public class LoginResponseDto
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? NombreCompleto { get; set; }
    public string? Email { get; set; }
    public string? Rol { get; set; }
    public string? Token { get; set; }
}
