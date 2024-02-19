using Newtonsoft.Json;
using unwinder.Models.AmadeusApiServiceModels.KeyModels;
using System;
using unwinder.Helpers;

namespace unwinder.Services;

/// <summary>
/// Service for obtaining and refreshing OAuth tokens for the Amadeus API.
/// </summary>
public class GetToken : IGetToken
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;
    private readonly string _serviceApiKey;
    private readonly string _serviceApiSecretKey;
    private BearerTokenModel _currentToken;
    private readonly SemaphoreSlim _refreshLock = new SemaphoreSlim(1, 1); // Lock for token refresh

    /// <summary>
    /// Initializes a new instance of the <see cref="GetToken"/> class.
    /// </summary>
    /// <param name="httpClientFactory">The HTTP client factory used to create HTTP clients.</param>
    /// <param name="logger">The logger for logging information and errors.</param>
    /// <param name="serviceApiKey">The API key for the service.</param>
    /// <param name="serviceApiSecretKey">The API secret key for the service.</param>
    public GetToken(IHttpClientFactory httpClientFactory, ILogger<GetToken> logger, string serviceApiKey, string serviceApiSecretKey)
    {
        _httpClient = httpClientFactory.CreateClient("AmadeusApiV1");
        _logger = logger;
        _serviceApiKey = serviceApiKey;
        _serviceApiSecretKey = serviceApiSecretKey;
    }

    /// <summary>
    /// Gets the authentication token. Refreshes the token if it is expired or about to expire.
    /// </summary>
    /// <returns>The authentication token as a string.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the API response is empty or lacks the expected access token.</exception>
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

    /// <summary>
    /// Refreshes the authentication token by making a request to the OAuth token endpoint.
    /// </summary>
    /// <remarks>
    /// This method updates the internal token storage with a new token and its expiry time.
    /// </remarks>
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

        if (string.IsNullOrEmpty(responseString) || responseString == "{}")
        {
            throw new InvalidOperationException("Api response is empty.");
        }


        _logger.LogInformation("Bearer response: {responseString}", responseString);

        var deserializedResponse = JsonConvert.DeserializeObject<BearerTokenModel>(responseString);
        if (deserializedResponse == null || deserializedResponse.access_token == null)
        {
            throw new JsonSerializationException(string.Format(ErrorMessages.UnexpectedJsonStructure, "missing access_token"));
        }

        // Set expiry time for the token
        deserializedResponse.ExpiryTime = DateTime.UtcNow.AddSeconds(deserializedResponse.expires_in);

        _currentToken = deserializedResponse;
    }
}