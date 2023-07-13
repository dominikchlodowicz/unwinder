using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using unwinder.Models;
using unwinder.Services;

namespace unwinder.Services;

public class AmadeusApiService : IAmadeusApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<IAmadeusApiService> _logger;
    private readonly IGetToken _bearerToken;

    public AmadeusApiService(IHttpClientFactory httpClientFactory, ILogger<IAmadeusApiService> logger, IGetToken bearerToken)
    {
        _httpClient = httpClientFactory.CreateClient("AmadeusApiV1");
        _logger = logger;
        _bearerToken = bearerToken;
    }
    
    // makes request to GetLocation api endpoint in Amadeus API
    // return name of airport, city and iata given city name
    public async Task<string> GetLocation(string query)
    {   
        var token = await GetToken();
        AddBearerTokenToHttpClient(token);
        var response = await GetResponseFromApi(query);

        return await ProcessResponse(response);
    }

    private async Task<string> GetToken()
    {
        var token = await _bearerToken.GetAuthToken();
        _logger.LogInformation("Bearer Token: {token}", token);
        return token;
    }

    private void AddBearerTokenToHttpClient(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    private async Task<HttpResponseMessage> GetResponseFromApi(string query)
    {
        var response = await _httpClient.GetAsync($"reference-data/locations?subType=AIRPORT&keyword={query}");
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"Failed to retrieve data from API. Status code: {response.StatusCode}");
            return null; // return null or a more appropriate default value, or consider a more specific exception
        }

        return response;
    }

    private async Task<string> ProcessResponse(HttpResponseMessage response)
    {
        var responseContent = await response.Content.ReadAsStringAsync();
        var responseJson = JObject.Parse(responseContent);

        var airports = ExtractAirportsFromResponse(responseJson);
        var serializedAirports = JsonConvert.SerializeObject(airports);

        return serializedAirports;
    }

    private IEnumerable<object> ExtractAirportsFromResponse(JObject responseJson)
    {
        if (responseJson["data"] != null)
        {
            return responseJson["data"]
                .Select(a => new
                {
                    Name = (string)a["name"],
                    IataCode = (string)a["iataCode"],
                    CityName = (string)a["address"]["cityName"]
                });
        }
        else
        {
            return Enumerable.Empty<object>();
        }
    }

    
    // example api call for flight data
    // used in search form
    public async Task<FlightSearchOutputModel> SearchFlights()
    {
        var token = await _bearerToken.GetAuthToken();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        // figure out a way to get this data from form 
        var parameters = new Dictionary<string, string>
        {
            {"originLocationCode", "WAW"},
            {"destinationLocationCode", "SEZ"},
            {"departureDate", "2023-05-02"},
            {"adults", "2"},
            {"max", "2"}
        };
        var requestUri = $"shopping/flight-offers?originLocationCode={parameters["originLocationCode"]}&" +
                            $"destinationLocationCode={parameters["destinationLocationCode"]}&" +
                            $"departureDate={parameters["departureDate"]}&" +
                            $"adults={parameters["adults"]}&" +
                            $"max={parameters["max"]}";
                            
        var response = await _httpClient.GetAsync(requestUri);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to retrieve data from API. Status code: {response.StatusCode}");
        }

        var responseString = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(responseString))
        {
            throw new Exception("API response is empty.");
        }

        //deserialize
        var data = JsonConvert.DeserializeObject<FlightSearchOutputModel>(responseString);
        if (data == null)
        {
            throw new Exception("Failed to deserialize API response.");
        }
        
        return data;
    }
}