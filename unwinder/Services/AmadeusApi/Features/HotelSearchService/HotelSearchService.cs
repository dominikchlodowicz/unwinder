using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using unwidner.Models.AmadeusApiServiceModels.HotelSearchModels;
using unwinder.Helpers;
using unwinder.Models.AmadeusApiServiceModels.FlightSearchModels.TypeHelperModels;
using unwinder.Services.HelperServices;

namespace unwinder.Services.AmadeusApiService.HotelSearch;

public class HotelSearchService : IHotelSearchService
{
    private readonly IGetToken _getToken;
    private readonly HttpClient _httpClientV3;
    private readonly ICurrencyConversionService _currencyConversionService;

    private readonly string hotelSearchEndpointUri = "shopping/hotel-offers";

    public HotelSearchService(IHttpClientFactory httpClientFactory, IGetToken getToken, ICurrencyConversionService currencyConversionService)
    {
        _httpClientV3 = httpClientFactory.CreateClient("AmadeusApiV3");
        _getToken = getToken;
        _currencyConversionService = currencyConversionService;
    }

    public async Task<HotelSearchOutputModel> SearchHotel
        (HotelSearchParametersModel hotelSearchParametersModel)
    {
        var token = await _getToken.GetAuthToken();
        _httpClientV3.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await GetHotelListFromApi(hotelSearchParametersModel);

        return await ProcessHotelSearchResponse(response);
    }

    private async Task<HttpResponseMessage> GetHotelListFromApi(HotelSearchParametersModel hotelSearchParameters)
    {
        var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
        query["hotelIds"] = String.Join(",", hotelSearchParameters.HotelIds);
        query["aduts"] = hotelSearchParameters.Adults.ToString();
        query["checkInDate"] = hotelSearchParameters.CheckInDate;
        query["checkOutDate"] = hotelSearchParameters.CheckOutDate;
        query["currency"] = hotelSearchParameters.Currency;
        string url = $"{_httpClientV3.BaseAddress}{hotelSearchEndpointUri}?{query}";

        var response = await _httpClientV3.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Received {response.StatusCode} from the server.");
        }

        return response;
    }

    private async Task<HotelSearchOutputModel> ProcessHotelSearchResponse(HttpResponseMessage response)
    {
        var responseContent = await response.Content.ReadAsStringAsync();
        var hotelSearchData = await DeserializeFlightSearchResponse(responseContent);

        _currencyConversionService.HotelConvertCurrrency(ref hotelSearchData);

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