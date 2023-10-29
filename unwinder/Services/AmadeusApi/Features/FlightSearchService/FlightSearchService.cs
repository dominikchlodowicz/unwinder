using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;
using unwinder.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection.Metadata;
using System.Reflection.Emit;

namespace unwinder.Services.AmadeusApiService.FlightSearch;

public class FlightSearchService : IFlightSearchService
{
    private readonly HttpClient _httpClientV2;
    private readonly IGetToken _getToken;

    public FlightSearchService(IHttpClientFactory httpClientFactory, IGetToken getToken)
    {
        _httpClientV2 = httpClientFactory.CreateClient("AmadeusApiV2");
        _getToken = getToken;
    }

    private readonly string flightSearchEndpointUri = "shopping/flight-offers";

    public async Task<FlightSearchOutputModel> FlightSearch(FlightSearchParameters flightSearchParameters)
    {
        var token = await _getToken.GetAuthToken();
        _httpClientV2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await GetFlightSearchFromApi(flightSearchParameters);

        return await ProcessFlightSearchResponse(response);
    }

    private async Task<HttpResponseMessage> GetFlightSearchFromApi(FlightSearchParameters flightSearchParameters)
    {

        var processedParameters = ProcessFlightSearchParameters(flightSearchParameters);
        var response = await _httpClientV2.PostAsync(flightSearchEndpointUri, processedParameters);

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
            throw new Exception(ErrorMessages.DeserializeError);
        }

        return Task.FromResult(data);
    }
}