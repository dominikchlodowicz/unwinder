using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;
using unwinder.Models.AmadeusApiServiceModels.GetLocationModels;

namespace unwinder.Services;

public class AmadeusApiService : IAmadeusApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<IAmadeusApiService> _logger;
    private readonly IGetToken _bearerToken;

    private readonly string getLocationEndpointUri = "reference-data/locations?subType=AIRPORT&keyword=";
    private readonly string flightSearchEndpointUri = "shopping/flight-offers";

    public AmadeusApiService(IHttpClientFactory httpClientFactory, ILogger<IAmadeusApiService> logger, IGetToken bearerToken)
    {
        _httpClient = httpClientFactory.CreateClient("AmadeusApiV2");
        _logger = logger;
        _bearerToken = bearerToken;
    }

    // GetLocation methods - START

    public async Task<string> GetLocation(string query)
    {
        var token = await _bearerToken.GetAuthToken();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await GetLocationFromApi(query);

        return await ProcessGetLocationResponse(response);
    }

    private async Task<HttpResponseMessage> GetLocationFromApi(string query)
    {
        var response = await _httpClient.GetAsync(getLocationEndpointUri + query);
        ValidateResponse(response);

        return response;
    }

    private async Task<string> ProcessGetLocationResponse(HttpResponseMessage response)
    {
        var responseContent = await response.Content.ReadAsStringAsync();
        var responseJson = JObject.Parse(responseContent);

        var airports = DeserializeGetLocationResponse(responseJson);
        var serializedAirports = JsonConvert.SerializeObject(airports);

        return responseContent;
    }

    private IEnumerable<GetLocationAirportModel> DeserializeGetLocationResponse(JObject responseJson)
    {
        if (responseJson["data"] != null)
        {
            return responseJson["data"]
                .Select(a => new GetLocationAirportModel
                {
                    Name = (string)a["name"],
                    IataCode = (string)a["iataCode"],
                    CityName = (string)a["address"]["cityName"]
                });
        }
        else
        {
            return Enumerable.Empty<GetLocationAirportModel>();
        }
    }

    // GetLocation methods - END

    // FlightSearch methods - START

    public async Task<FlightSearchOutputModel> FlightSearch(FlightSearchParameters flightSearchParameters)
    {
        var token = await _bearerToken.GetAuthToken();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await GetFlightSearchFromApi(flightSearchParameters);

        return await ProcessFlightSearchResponse(response);
    }

    private async Task<HttpResponseMessage> GetFlightSearchFromApi(FlightSearchParameters flightSearchParameters)
    {

        var processedParameters = ProcessFlightSearchParameters(flightSearchParameters);

        var response = await _httpClient.PostAsync(flightSearchEndpointUri, processedParameters);

        ValidateResponse(response);

        return response;
    }

    private StringContent ProcessFlightSearchParameters(FlightSearchParameters flightSearchParameters)
    {
        var json = JsonConvert.SerializeObject(flightSearchParameters);
        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

        return stringContent;
    }

    private async Task<FlightSearchOutputModel> ProcessFlightSearchResponse(HttpResponseMessage response)
    {
        var responseContent = await response.Content.ReadAsStringAsync();

        var flightSearchData = await DeserializeFlightSearchResponse(responseContent);

        return flightSearchData;
    }


    private Task<FlightSearchOutputModel> DeserializeFlightSearchResponse(string flightResponse)
    {
        var data = JsonConvert.DeserializeObject<FlightSearchOutputModel>(flightResponse);
        if (data == null)
        {
            throw new Exception("Failed to deserialize API response.");
        }

        return Task.FromResult(data);
    }

    // FlightSearch methods - END


    private void ValidateResponse(HttpResponseMessage response)
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
}


