using System.Net.Http.Headers;
using System.Collections.Generic;
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

    public async Task<string> GetLocation(string query)
    {
        var token = await GetToken();
        AddBearerTokenToHttpClient(token);
        var response = await GetLocationFromApi(query);

        return await ProcessGetLocationResponse(response);
    }

    public async Task<List<FlightSearchOutputListModel>> FlightSearch(FlightSearchParameters flightSearchParameters)
    {
        var token = await GetToken();
        AddBearerTokenToHttpClient(token);
        var response = await GetFlightSearchFromApi(flightSearchParameters);

        return await ProcessFlightSearchResponse(response);
    }

    // Universal helper methods
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


    // Get Location methods
    private async Task<HttpResponseMessage> GetLocationFromApi(string query)
    {
        var response = await _httpClient.GetAsync(getLocationEndpointUri + query);
        ValidateResponse(response);

        return response;
    }

    private async Task<string> ProcessGetLocationResponse(HttpResponseMessage response)
    {
        var responseContent = await response.Content.ReadAsStringAsync();
        // var responseJson = JObject.Parse(responseContent);

        // var airports = DeserializeGetLocationResponse(responseJson);
        // var serializedAirports = JsonConvert.SerializeObject(airports);

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

    // FlightSearch methods

    private async Task<List<FlightSearchOutputListModel>> ProcessFlightSearchResponse(HttpResponseMessage response)
    {
        var responseContent = await response.Content.ReadAsStringAsync();

        var flightSearchData = await DeserializeFlightSearchResponse(responseContent);

        return flightSearchData.Data;
    }
    
    private async Task<HttpResponseMessage> GetFlightSearchFromApi(FlightSearchParameters flightSearchParameters)
    {
        var requestContent = BuildFlightSearchRequestUri(flightSearchParameters);

        _logger.LogInformation("Request Content: {requestContent}", requestContent);

        var response = await _httpClient.PostAsync(flightSearchEndpointUri, requestContent);

        ValidateResponse(response);

        return response;
    }

    private HttpContent BuildFlightSearchRequestUri(FlightSearchParameters flightSearchParameters)
    {
    var parameters = new Dictionary<string, string>
    {
        { "originLocationCode", flightSearchParameters.OriginLocationCode },
        { "destinationLocationCode", flightSearchParameters.DestinationLocationCode },
        { "departureDate", flightSearchParameters.DepartureDate },
        { "adults", flightSearchParameters.Adults.ToString() },
        { "max", flightSearchParameters.Max.ToString() }
    };

    return new FormUrlEncodedContent(parameters);
    }

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

    private Task<RootObject> DeserializeFlightSearchResponse(string flightResponse)
    {
        var data = JsonConvert.DeserializeObject<RootObject>(flightResponse);
        if (data == null)
        {
            throw new Exception("Failed to deserialize API response.");
        }

        return Task.FromResult(data);
    }

    // private IEnumerable<FlightSearchOutputModel> DeserializeFlightSearchResponse(JObject responseJson)
    // {
    //     if (responseJson["data"] != null)
    //     {
    //         return responseJson["data"]
    //             .Select(a => new FlightSearchOutputModel
    //             {
    //                 Name = (string)a["name"],
    //                 IataCode = (string)a["iataCode"],
    //                 CityName = (string)a["address"]["cityName"]
    //             });
    //     }
    //     else
    //     {
    //         return Enumerable.Empty<FlightSearchOutputModel>();
    //     }
    // }


}


