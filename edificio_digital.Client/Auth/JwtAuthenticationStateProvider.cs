using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;

namespace edificio_digital.Client.Auth;

public class JwtAuthenticationStateProvider(TokenStorage tokenStorage) : AuthenticationStateProvider
{
    private static readonly AuthenticationState Anonymous =
        new(new ClaimsPrincipal(new ClaimsIdentity()));

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await tokenStorage.GetAccessTokenAsync();
        if (string.IsNullOrEmpty(token)) return Anonymous;

        try
        {
            var claims = ParseClaimsFromJwt(token);

            var exp = claims.FirstOrDefault(c => c.Type == "exp")?.Value;
            if (long.TryParse(exp, out var expSeconds))
            {
                var expDate = DateTimeOffset.FromUnixTimeSeconds(expSeconds);
                if (expDate <= DateTimeOffset.UtcNow)
                    return Anonymous;
            }

            var identity = new ClaimsIdentity(claims, "jwt", ClaimTypes.Name, ClaimTypes.Role);
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
        catch
        {
            return Anonymous;
        }
    }

    public void NotifyUserChanged() =>
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

    public void NotifySignedOut() =>
        NotifyAuthenticationStateChanged(Task.FromResult(Anonymous));

    private static List<Claim> ParseClaimsFromJwt(string jwt)
    {
        var parts = jwt.Split('.');
        if (parts.Length < 2) return [];

        var payload = parts[1];
        var bytes = Convert.FromBase64String(PadBase64(payload));
        var dict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(bytes);
        if (dict is null) return [];

        var claims = new List<Claim>();
        foreach (var (key, value) in dict)
        {
            var type = MapClaimType(key);
            if (value.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in value.EnumerateArray())
                    claims.Add(new Claim(type, item.ToString()));
            }
            else
            {
                claims.Add(new Claim(type, value.ToString()));
            }
        }
        return claims;
    }

    private static string MapClaimType(string jwtKey) => jwtKey switch
    {
        "role" => ClaimTypes.Role,
        "roles" => ClaimTypes.Role,
        "name" => ClaimTypes.Name,
        "sub" => ClaimTypes.NameIdentifier,
        _ => jwtKey
    };

    private static string PadBase64(string s)
    {
        s = s.Replace('-', '+').Replace('_', '/');
        return (s.Length % 4) switch
        {
            2 => s + "==",
            3 => s + "=",
            _ => s
        };
    }
}
