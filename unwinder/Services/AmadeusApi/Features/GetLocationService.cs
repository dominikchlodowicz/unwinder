using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using unwinder.Models.AmadeusApiServiceModels.GetLocationModels;

namespace unwinder.Services.AmadeusApiService;

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

        if(string.IsNullOrEmpty(responseContent) || responseContent == "{}")
        {
            throw new InvalidOperationException("Api response is empty.");
        }

        var responseJson = JObject.Parse(responseContent);

        var airports = DeserializeGetLocationResponse(responseJson);

        return airports;
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
            throw new InvalidOperationException("Unexpected JSON structure: missing data property.");
        }
    }
}