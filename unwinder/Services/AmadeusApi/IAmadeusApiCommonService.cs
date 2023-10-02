namespace unwinder.Services.AmadeusApiService;

public interface IAmadeusApiCommonService 
{
    public void ValidateResponse(HttpResponseMessage response);

    public Task<string> GetAuthToken();

    public HttpClient GetHttpClientV1();

    public HttpClient GetHttpClientV2();
}