using Newtonsoft.Json;
using unwinder.Models.AmadeusApiServiceModels.KeyModels;
using System;
using unwinder.Helpers;

namespace unwinder.Services;

public class GetToken : IGetToken
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;
    private readonly string _serviceApiKey;
    private readonly string _serviceApiSecretKey;
    private BearerTokenModel _currentToken;
    private readonly SemaphoreSlim _refreshLock = new SemaphoreSlim(1, 1); // Lock for token refresh


    public GetToken(IHttpClientFactory httpClientFactory, ILogger<GetToken> logger, string serviceApiKey, string serviceApiSecretKey)
    {
        _httpClient = httpClientFactory.CreateClient("AmadeusApiV1");
        _logger = logger;
        _serviceApiKey = serviceApiKey;
        _serviceApiSecretKey = serviceApiSecretKey;
    }

    public async Task<string> GetAuthToken()
    {
        // Check if token is expired or about to expire
        if (_currentToken == null || DateTime.UtcNow >= _currentToken.ExpiryTime)
        {
            await _refreshLock.WaitAsync(); // Wait for the lock
            try
            {
                // Double-check the token state to see if it was refreshed while waiting for the lock
                if (_currentToken == null || DateTime.UtcNow >= _currentToken.ExpiryTime)
                {
                    await RefreshToken();
                }
            }
            finally
            {
                _refreshLock.Release(); // Release the lock
            }
        }

        return _currentToken.access_token;
    }

    private async Task RefreshToken()
    {
        _logger.LogInformation("Refreshing API token...");

        var parameters = new Dictionary<string, string>
        {
            {"grant_type", "client_credentials"},
            {"client_id", _serviceApiKey},
            {"client_secret", _serviceApiSecretKey}
        };

        var content = new FormUrlEncodedContent(parameters);
        var response = await _httpClient.PostAsync("security/oauth2/token", content);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Received error code {response.StatusCode}");
        }

        var responseString = await response.Content.ReadAsStringAsync();
        _logger.LogInformation("Bearer response: {responseString}", responseString);

        var deserializedResponse = JsonConvert.DeserializeObject<BearerTokenModel>(responseString);
        if (deserializedResponse == null || deserializedResponse.access_token == null)
        {
            throw new InvalidOperationException("Failed to deserialize API response.");
        }

        // Set expiry time for the token
        deserializedResponse.ExpiryTime = DateTime.UtcNow.AddSeconds(deserializedResponse.expires_in);

        _currentToken = deserializedResponse;
    }
}