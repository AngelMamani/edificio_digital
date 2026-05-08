using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace edificio_digital.Client.Auth;

public class AuthorizationMessageHandler(
    TokenStorage tokenStorage,
    JwtAuthenticationStateProvider authState) : DelegatingHandler
{
    private static readonly string[] SkipPaths =
    [
        "/api/auth/login",
        "/api/auth/refresh"
    ];

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken ct)
    {
        var skip = ShouldSkip(request);

        if (!skip)
        {
            var token = await tokenStorage.GetAccessTokenAsync();
            if (!string.IsNullOrEmpty(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await base.SendAsync(request, ct);

        if (skip || response.StatusCode != HttpStatusCode.Unauthorized)
            return response;

        var refreshed = await TryRefreshAsync(ct);
        if (!refreshed)
        {
            await tokenStorage.RemoveAccessTokenAsync();
            authState.NotifySignedOut();
            return response;
        }

        response.Dispose();
        var retry = await CloneRequestAsync(request, ct);
        var newToken = await tokenStorage.GetAccessTokenAsync();
        if (!string.IsNullOrEmpty(newToken))
            retry.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newToken);
        return await base.SendAsync(retry, ct);
    }

    private static bool ShouldSkip(HttpRequestMessage request)
    {
        var path = request.RequestUri?.AbsolutePath;
        return path is not null && SkipPaths.Any(p => path.EndsWith(p, StringComparison.OrdinalIgnoreCase));
    }

    private async Task<bool> TryRefreshAsync(CancellationToken ct)
    {
        try
        {
            using var req = new HttpRequestMessage(HttpMethod.Post, "api/auth/refresh");
            using var resp = await base.SendAsync(req, ct);
            if (!resp.IsSuccessStatusCode) return false;

            var data = await resp.Content.ReadFromJsonAsync<RefreshResult>(cancellationToken: ct);
            if (data?.AccessToken is null) return false;

            await tokenStorage.SetAccessTokenAsync(data.AccessToken);
            authState.NotifyUserChanged();
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static async Task<HttpRequestMessage> CloneRequestAsync(HttpRequestMessage src, CancellationToken ct)
    {
        var clone = new HttpRequestMessage(src.Method, src.RequestUri);

        if (src.Content is not null)
        {
            var bytes = await src.Content.ReadAsByteArrayAsync(ct);
            clone.Content = new ByteArrayContent(bytes);
            foreach (var h in src.Content.Headers)
                clone.Content.Headers.TryAddWithoutValidation(h.Key, h.Value);
        }

        foreach (var h in src.Headers.Where(h => !string.Equals(h.Key, "Authorization", StringComparison.OrdinalIgnoreCase)))
            clone.Headers.TryAddWithoutValidation(h.Key, h.Value);

        return clone;
    }

    private record RefreshResult(string AccessToken, DateTime AccessTokenExpiresAt);
}
