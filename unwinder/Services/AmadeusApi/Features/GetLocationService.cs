using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using unwinder.Models.AmadeusApiServiceModels.GetLocationModels;

namespace unwinder.Services.AmadeusApiService;

public class GetLocationService : IGetLocationService
{
    private readonly IAmadeusApiCommonService _commonService;

    private readonly string getLocationEndpointUri = "reference-data/locations?subType=AIRPORT&keyword=";

    public GetLocationService(IAmadeusApiCommonService commonService)
    {
        _commonService = commonService;
    }

    public async Task<IEnumerable<GetLocationAirportModel>> GetLocation(string query)
    {
        var token = await _commonService.GetAuthToken();
        _commonService.GetHttpClientV1().DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await GetLocationFromApi(query);

        return await ProcessGetLocationResponse(response);
    }

    private async Task<HttpResponseMessage> GetLocationFromApi(string query)
    {   
        string httpQuery = getLocationEndpointUri + query;
        var response = await _commonService.GetHttpClientV2().GetAsync(httpQuery);
        _commonService.ValidateResponse(response);

        return response;
    }

    private async Task<IEnumerable<GetLocationAirportModel>> ProcessGetLocationResponse(HttpResponseMessage response)
    {
        var responseContent = await response.Content.ReadAsStringAsync();
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
            return Enumerable.Empty<GetLocationAirportModel>();
        }
    }
}