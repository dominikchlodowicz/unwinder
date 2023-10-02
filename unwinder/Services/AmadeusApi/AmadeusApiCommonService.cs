namespace unwinder.Services.AmadeusApiService;

public class AmadeusApiCommonService : IAmadeusApiCommonService
{
    private readonly HttpClient _httpClientV1;
    private readonly HttpClient _httpClientV2;
    private readonly ILogger<IAmadeusApiCommonService> _logger;
    private readonly IGetToken _bearerToken;

    public AmadeusApiCommonService(IHttpClientFactory httpClientFactory, ILogger<IAmadeusApiCommonService> logger, IGetToken bearerToken)
    {
        _httpClientV1 = httpClientFactory.CreateClient("AmadeusApiV1");
        _httpClientV2 = httpClientFactory.CreateClient("AmadeusApiV2");
        _logger = logger;
        _bearerToken = bearerToken;
    }

    public void ValidateResponse(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to retrieve data from API. Status code: {response.StatusCode}");
        }

        if (response.Content == null || response.Content.ReadAsStringAsync().Result == null)
        {
            throw new Exception("API response is empty.");
        }
    }

    public async Task<string> GetAuthToken()
    {
        return await _bearerToken.GetAuthToken();
    }

    public HttpClient GetHttpClientV1()
    {
        return _httpClientV1;
    }

    public HttpClient GetHttpClientV2()
    {
        return _httpClientV2;
    }

}