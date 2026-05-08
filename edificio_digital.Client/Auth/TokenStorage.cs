using Microsoft.JSInterop;

namespace edificio_digital.Client.Auth;

public class TokenStorage(IJSRuntime js)
{
    public const string AccessTokenKey = "ed_access_token";

    public async Task<string?> GetAccessTokenAsync()
    {
        try
        {
            return await js.InvokeAsync<string?>("localStorage.getItem", AccessTokenKey);
        }
        catch
        {
            return null;
        }
    }

    public async Task SetAccessTokenAsync(string token) =>
        await js.InvokeVoidAsync("localStorage.setItem", AccessTokenKey, token);

    public async Task RemoveAccessTokenAsync() =>
        await js.InvokeVoidAsync("localStorage.removeItem", AccessTokenKey);
}
