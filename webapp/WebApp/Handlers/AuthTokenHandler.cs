using System.Net.Http.Headers;

namespace SCISalesTest.WebApp.Handlers;

public class AuthTokenHandler : DelegatingHandler
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthTokenHandler> _logger;
    private static readonly SemaphoreSlim _semaphore = new(1, 1);
    private static string? _cachedToken;
    private static DateTime _tokenExpiration;

    public AuthTokenHandler(IConfiguration configuration, ILogger<AuthTokenHandler> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.RequestUri?.PathAndQuery.Contains("api/auth/login", StringComparison.OrdinalIgnoreCase) == true)
        {
            return await base.SendAsync(request, cancellationToken);
        }

        await EnsureTokenAsync(request, cancellationToken);

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            _cachedToken = null;
            await EnsureTokenAsync(request, cancellationToken);
            response = await base.SendAsync(request, cancellationToken);
        }

        return response;
    }

    private async Task EnsureTokenAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(_cachedToken) && DateTime.UtcNow < _tokenExpiration)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _cachedToken);
            return;
        }

        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            if (!string.IsNullOrEmpty(_cachedToken) && DateTime.UtcNow < _tokenExpiration)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _cachedToken);
                return;
            }

            var baseAddress = _configuration["WebApi:BaseAddress"] ?? "https://localhost:5000";
            using var loginClient = new HttpClient { BaseAddress = new Uri(baseAddress) };

            var loginResponse = await loginClient.PostAsJsonAsync("api/auth/login", new
            {
                Username = _configuration["WebApi:Username"] ?? "admin",
                Password = _configuration["WebApi:Password"] ?? "Admin123!"
            }, cancellationToken);

            if (loginResponse.IsSuccessStatusCode)
            {
                var result = await loginResponse.Content.ReadFromJsonAsync<LoginApiResponse>(cancellationToken: cancellationToken);
                if (result is not null)
                {
                    _cachedToken = result.Token;
                    _tokenExpiration = result.Expiration.AddMinutes(-5);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _cachedToken);
                    _logger.LogInformation("Bearer token obtained successfully, expires at {Expiration}", result.Expiration);
                }
            }
            else
            {
                _logger.LogWarning("Failed to obtain bearer token. Status: {StatusCode}", loginResponse.StatusCode);
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private class LoginApiResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
    }
}
