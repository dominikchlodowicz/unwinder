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

        HotelConvertCurrrency(ref hotelSearchData);

        return hotelSearchData;
    }

    private void HotelConvertCurrrency(ref HotelSearchOutputModel hotelSearchOutputData)
    {
        string currency = hotelSearchOutputData.Data[0].Offers[0].Price.Currency;
        int priceValue = int.Parse(hotelSearchOutputData.Data[0].Offers[0].Price.Total);

        if (hotelSearchOutputData.Dictionaries != null)
        {
            decimal priceValueDecimal = priceValue;

            decimal conversionRate = decimal.Parse(hotelSearchOutputData.Dictionaries.CurrencyConversionLookupRates[currency].Rate);
            string currencyTarget = hotelSearchOutputData.Dictionaries.CurrencyConversionLookupRates[currency].Target;

            hotelSearchOutputData.ConvertedCurrencyPrice.CurrencyCode = currencyTarget;
            hotelSearchOutputData.ConvertedCurrencyPrice.Value = (int)Math.Round(priceValueDecimal * conversionRate); ;
        }
        else
        {
            hotelSearchOutputData.ConvertedCurrencyPrice.CurrencyCode = currency;
            hotelSearchOutputData.ConvertedCurrencyPrice.Value = priceValue;
        }
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