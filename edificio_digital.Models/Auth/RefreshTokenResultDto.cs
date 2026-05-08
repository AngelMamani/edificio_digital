namespace edificio_digital.Models.Auth;

public class RefreshTokenResultDto
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? AccessToken { get; set; }
    public DateTime? AccessTokenExpiresAt { get; set; }
    public string? NewRefreshToken { get; set; }
    public DateTime? NewRefreshTokenExpiresAt { get; set; }
}
