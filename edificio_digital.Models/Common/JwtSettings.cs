namespace edificio_digital.Models.Common;

public class JwtSettings
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = "edificio-digital";
    public string Audience { get; set; } = "edificio-digital";
    public string Key { get; set; } = string.Empty;
    public int AccessTokenMinutes { get; set; } = 15;
    public int RefreshTokenDays { get; set; } = 7;
    public string RefreshCookieName { get; set; } = "ed_refresh";
    public string RefreshCookiePath { get; set; } = "/api/auth";
}
