using Newtonsoft.Json;
using unwinder.Models.AmadeusApiServiceModels.KeyModels;
using System;

namespace unwinder.Services;

public class GetToken : IGetToken
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;

    private readonly string _serviceApiKey;
    private readonly string _serviceApiSecretKey;


    public GetToken(IHttpClientFactory httpClientFactory, ILogger<GetToken> logger, string serviceApiKey, string serviceApiSecretKey)
    {
        _httpClient = httpClientFactory.CreateClient("AmadeusApiV1");
        _logger = logger;
        _serviceApiKey = serviceApiKey;
        _serviceApiSecretKey = serviceApiSecretKey;
    }

    public async Task<string> GetAuthToken()
    {
        _logger.LogInformation("Api keys: {serviceApiKey}, {serviceApiSecretKey}", _serviceApiKey, _serviceApiSecretKey);
        var parameters = new Dictionary<string, string>
        {
            {"grant_type", "client_credentials"},
            {"client_id", $"{_serviceApiKey}"},
            {"client_secret", $"{_serviceApiSecretKey}"}
        };

        var content = new FormUrlEncodedContent(parameters);

        var response = await _httpClient.PostAsync("security/oauth2/token", content);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Received {response.StatusCode} from the server.");
        }

        var responseString = await response.Content.ReadAsStringAsync();

        if(string.IsNullOrEmpty(responseString) || responseString == "{}")
        {
            throw new InvalidOperationException("Api response is empty.");
        }

        _logger.LogInformation("Bearer response: {responseString}", responseString);

        var deserializedResponse = JsonConvert.DeserializeObject<BearerTokenModel>(responseString);

        if (deserializedResponse?.access_token == null)
        {
            throw new JsonSerializationException("Unexpected JSON structure: missing access_token.");
        }

        return deserializedResponse != null ? deserializedResponse.access_token : "Error";
    }
}