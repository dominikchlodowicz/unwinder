using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using unwidner.Models.AmadeusApiServiceModels.HotelSearchModels;
using unwinder.Helpers;

namespace unwinder.Services.AmadeusApiService.HotelSearch;

public class HotelSearchListService : IHotelSearchListService
{
    private IGetToken _getToken;
    private HttpClient _httpClientV1;
    private readonly string hotelListSearchEndpointUri = "reference-data/locations/hotels/by-city";

    public HotelSearchListService(IHttpClientFactory httpClientFactory, IGetToken getToken)
    {
        _httpClientV1 = httpClientFactory.CreateClient("AmadeusApiV1");
        _getToken = getToken;
    }

    public async Task<HotelSearchListOutputModel> SearchListOfHotels
            (HotelSearchListParametersModel hotelSearchListParametersModel)
    {
        var token = await _getToken.GetAuthToken();
        _httpClientV1.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await GetHotelListFromApi(hotelSearchListParametersModel);

        return await ProcessHotelListSearchResponse(response);
    }

    private async Task<HttpResponseMessage> GetHotelListFromApi(HotelSearchListParametersModel hotelSearchListParametersModel)
    {
        // var processedParameters = ProcessHotelSearchListParameters(hotelSearchListParametersModel);
        var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
        query["cityCode"] = hotelSearchListParametersModel.CityCode;
        query["radius"] = hotelSearchListParametersModel.Radius.ToString();
        string url = $"{_httpClientV1.BaseAddress}{hotelListSearchEndpointUri}?{query}";

        var response = await _httpClientV1.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Received {response.StatusCode} from the server.");
        }

        return response;
    }

    private async Task<HotelSearchListOutputModel> ProcessHotelListSearchResponse(HttpResponseMessage response)
    {
        var responseContent = await response.Content.ReadAsStringAsync();
        var hotelListSearchData = await DeserializeFlightSearchResponse(responseContent);

        return hotelListSearchData;
    }

    private Task<HotelSearchListOutputModel> DeserializeFlightSearchResponse(string hotelListResponse)
    {
        var data = JsonConvert.DeserializeObject<HotelSearchListOutputModel>(hotelListResponse);
        if (data == null)
        {
            throw new Exception(ErrorMessages.DeserializeError);
        }

        return Task.FromResult(data);
    }
}