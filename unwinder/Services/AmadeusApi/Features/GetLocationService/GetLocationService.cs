using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using unwinder.Models.AmadeusApiServiceModels.GetLocationModels;

namespace unwinder.Services.AmadeusApiService.GetLocation;

/// <summary>
/// Provides the implementation for retrieving location information based on a search query.
/// </summary>
public class GetLocationService : IGetLocationService
{

    private readonly HttpClient _httpClientV1;
    private readonly IGetToken _getToken;

    private readonly string getLocationEndpointUri = "reference-data/locations?subType=AIRPORT&keyword=";

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLocationService"/> class.
    /// </summary>
    /// <param name="httpClientFactory">The factory used to create HTTP client instances.</param>
    /// <param name="getToken">The service used to authenticate API requests.</param>
    public GetLocationService(IHttpClientFactory httpClientFactory, IGetToken getToken)
    {
        _httpClientV1 = httpClientFactory.CreateClient("AmadeusApiV1");
        _getToken = getToken;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GetLocationAirportModel>> GetLocation(string query)
    {
        var token = await _getToken.GetAuthToken();
        _httpClientV1.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await GetLocationFromApi(query);

        return await ProcessGetLocationResponse(response);
    }

    /// <summary>
    /// Sends a request to the API to retrieve location data based on the specified query.
    /// </summary>
    /// <param name="query">The search query for finding locations, typically an airport name, city name, or an IATA code.</param>
    /// <returns>A HttpResponseMessage containing the API response.</returns>
    /// <exception cref="HttpRequestException">Thrown if the response from the server is not a success status code.</exception>

    private async Task<HttpResponseMessage> GetLocationFromApi(string query)
    {
        string httpQuery = getLocationEndpointUri + query;
        var response = await _httpClientV1.GetAsync(httpQuery);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Received {response.StatusCode} from the server.");
        }

        return response;
    }

    /// <summary>
    /// Processes the API response, parsing the JSON content and converting it into a collection of GetLocationAirportModel objects.
    /// </summary>
    /// <param name="response">The HttpResponseMessage received from the API.</param>
    /// <returns>A collection of GetLocationAirportModel objects representing the airports found in the API response.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the API response is empty or if the expected data structure is not found.</exception>
    private async Task<IEnumerable<GetLocationAirportModel>> ProcessGetLocationResponse(HttpResponseMessage response)
    {
        var responseContent = await response.Content.ReadAsStringAsync();

        var responseJson = JObject.Parse(responseContent);

        if (responseJson["data"] is JArray dataArray && !dataArray.Any())
        {
            throw new InvalidOperationException("Api response is empty.");
        }

        var airports = DeserializeGetLocationResponse(responseJson);

        return airports;
    }

    /// <summary>
    /// Deserializes the JSON object into a collection of GetLocationAirportModel objects.
    /// </summary>
    /// <param name="responseJson">The JObject parsed from the API response content.</param>
    /// <returns>A collection of GetLocationAirportModel objects representing the data contained in the response JSON.</returns>
    /// <exception cref="InvalidOperationException">Thrown if required properties are missing in the JSON object.</exception>
    private IEnumerable<GetLocationAirportModel> DeserializeGetLocationResponse(JObject responseJson)
    {
        var results = new List<GetLocationAirportModel>();
        foreach (var a in responseJson["data"])
        {
            // Debugging is easier here
            var name = (string)a["name"];
            var iataCode = (string)a["iataCode"];
            var cityName = (string)a["address"]["cityName"];
            var cityCode = (string)a["address"]["cityCode"];
            var countryName = (string)a["address"]["countryName"];

            if (name == null || iataCode == null || cityName == null)
            {
                throw new InvalidOperationException("Unexpected JSON structure: Some required properties are missing.");
            }

            results.Add(new GetLocationAirportModel
            {
                Name = name,
                IataCode = iataCode,
                CityName = cityName,
                CityCode = cityCode,
                CountryName = countryName
            });
        }
        return results;
    }
}