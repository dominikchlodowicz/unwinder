using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using unwidner.Models.AmadeusApiServiceModels.HotelSearchModels;
using unwinder.Helpers;

namespace unwinder.Services.AmadeusApiService.HotelSearch;

public class HotelSearchService : IHotelSearchService
{
    private IGetToken _getToken;
    private HttpClient _httpClientV3;

    private readonly string hotelSearchEndpointUri = "shopping/hotel-offers";

    public HotelSearchService(IHttpClientFactory httpClientFactory, IGetToken getToken)
    {
        _httpClientV3 = httpClientFactory.CreateClient("AmadeusApiV3");
        _getToken = getToken;
    }

    public async Task<HotelSearchOutputModel> SearchHotel
        (HotelSearchParametersModel hotelSearchParametersModel)
    {
        var token = await _getToken.GetAuthToken();
        _httpClientV3.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await GetHotelListFromApi(hotelSearchParametersModel);

        return await ProcessHotelSearchResponse(response);
    }

    private async Task<HttpResponseMessage> GetHotelListFromApi(HotelSearchParametersModel hotelSearchParametersModel)
    {
        var processedParameters = ProcessHotelSearchParameters(hotelSearchParametersModel);
        var response = await _httpClientV3.PostAsync(hotelSearchEndpointUri, processedParameters);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Received {response.StatusCode} from the server.");
        }

        return response;
    }

    private StringContent ProcessHotelSearchParameters(HotelSearchParametersModel hotelSearchParameters)
    {
        var json = JsonConvert.SerializeObject(hotelSearchParameters);
        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

        return stringContent;
    }

    private async Task<HotelSearchOutputModel> ProcessHotelSearchResponse(HttpResponseMessage response)
    {
        var responseContent = await response.Content.ReadAsStringAsync();
        var hotelSearchData = await DeserializeFlightSearchResponse(responseContent);

        return hotelSearchData;
    }

    private Task<HotelSearchOutputModel> DeserializeFlightSearchResponse(string hotelResponse)
    {
        var data = JsonConvert.DeserializeObject<HotelSearchOutputModel>(hotelResponse);
        if (data == null)
        {
            throw new Exception(ErrorMessages.DeserializeError);
        }

        return Task.FromResult(data);
    }
}