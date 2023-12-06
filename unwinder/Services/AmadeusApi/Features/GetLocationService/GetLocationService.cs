using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using unwinder.Models.AmadeusApiServiceModels.GetLocationModels;

namespace unwinder.Services.AmadeusApiService.GetLocation;

public class GetLocationService : IGetLocationService
{

    private readonly HttpClient _httpClientV1;
    private readonly IGetToken _getToken;

    private readonly string getLocationEndpointUri = "reference-data/locations?subType=AIRPORT&keyword=";

    public GetLocationService(IHttpClientFactory httpClientFactory, IGetToken getToken)
    {
        _httpClientV1 = httpClientFactory.CreateClient("AmadeusApiV1");
        _getToken = getToken;
    }

    public async Task<IEnumerable<GetLocationAirportModel>> GetLocation(string query)
    {
        var token = await _getToken.GetAuthToken();
        _httpClientV1.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await GetLocationFromApi(query);

        return await ProcessGetLocationResponse(response);
    }

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

    private IEnumerable<GetLocationAirportModel> DeserializeGetLocationResponse(JObject responseJson)
    {
        var results = new List<GetLocationAirportModel>();
        foreach (var a in responseJson["data"])
        {
            // Debugging is easier here
            var name = (string)a["name"];
            var iataCode = (string)a["iataCode"];
            var cityName = (string)a["address"]["cityName"];
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
                CountryName = countryName
            });
        }
        return results;
    }
}