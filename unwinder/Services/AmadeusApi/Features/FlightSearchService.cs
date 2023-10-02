using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;

namespace unwinder.Services.AmadeusApiService;

public class FlightSearchService : IFlightSearchService
{
    private readonly IAmadeusApiCommonService _commonService;

    public FlightSearchService(IAmadeusApiCommonService commonService)
    {
        _commonService = commonService;
    }

    private readonly string flightSearchEndpointUri = "shopping/flight-offers";

    public async Task<FlightSearchOutputModel> FlightSearch(FlightSearchParameters flightSearchParameters)
    {
        var token = await _commonService.GetAuthToken();
        _commonService.GetHttpClientV2().DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await GetFlightSearchFromApi(flightSearchParameters);

        return await ProcessFlightSearchResponse(response);
    }

    private async Task<HttpResponseMessage> GetFlightSearchFromApi(FlightSearchParameters flightSearchParameters)
    {

        var processedParameters = ProcessFlightSearchParameters(flightSearchParameters);

        var response = await _commonService.GetHttpClientV2().PostAsync(flightSearchEndpointUri, processedParameters);

        _commonService.ValidateResponse(response);

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
}