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


/// <summary>
/// Service responsible for executing flight searches through the Amadeus API.
/// </summary>
public class FlightSearchService : IFlightSearchService
{
    private readonly HttpClient _httpClientV2;
    private readonly IGetToken _getToken;
    private readonly string flightSearchEndpointUri = "shopping/flight-offers";

    /// <summary>
    /// Initializes a new instance of the <see cref="FlightSearchService"/> class.
    /// </summary>
    /// <param name="httpClientFactory">The HTTP client factory used to create HTTP clients.</param>
    /// <param name="getToken">Service that contains OAuth token for Api authentication.</param>
    public FlightSearchService(IHttpClientFactory httpClientFactory, IGetToken getToken)
    {
        _httpClientV2 = httpClientFactory.CreateClient("AmadeusApiV2");
        _getToken = getToken;
    }

    /// <summary>
    /// Searches for flight offer and procesess obtained response.
    /// </summary>
    /// <param name="flightSearchParameters">Parameters for flight search as FlightSearchParameters object.</param>
    /// <returns>Processed flight response as FlightSearchOutputModel object.</returns>
    public async Task<FlightSearchOutputModel> FlightSearch(FlightSearchParameters flightSearchParameters)
    {
        var token = await _getToken.GetAuthToken();
        _httpClientV2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await GetFlightSearchFromApi(flightSearchParameters);

        return await ProcessFlightSearchResponse(response);
    }

    /// <summary>
    /// Search for flight offers in the Amadeus Api endpoint.
    /// </summary>
    /// <param name="flightSearchParameters">Parameters for flight search as FlightSearchParameters object.</param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException">Thrown if API response status is not 200.</exception>
    private async Task<HttpResponseMessage> GetFlightSearchFromApi(FlightSearchParameters flightSearchParameters)
    {

        var processedParameters = ProcessFlightSearchParameters(flightSearchParameters);
        var response = await _httpClientV2.PostAsync(flightSearchEndpointUri, processedParameters);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Received {response.StatusCode} from the server.");
        }

        return response;
    }

    /// <summary>
    /// Serializes flight search parameters into a JSON StringContent.
    /// </summary>
    /// <param name="flightSearchParameters">The flight search parameters to be serialized.</param>
    /// <returns>A StringContent object containing the JSON representation of the flight search parameters, encoded in UTF8 and with a content type of application/json.</returns>
    private StringContent ProcessFlightSearchParameters(FlightSearchParameters flightSearchParameters)
    {
        var json = JsonConvert.SerializeObject(flightSearchParameters);
        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

        return stringContent;
    }

    /// <summary>
    /// Processes the HttpResponseMessage from a flight search request by deserializing its content into a FlightSearchOutputModel.
    /// </summary>
    /// <param name="response">The HTTP response message received from the flight search request.</param>
    /// <returns>A Task that represents the asynchronous operation, resulting in the deserialized FlightSearchOutputModel.</returns>
    private async Task<FlightSearchOutputModel> ProcessFlightSearchResponse(HttpResponseMessage response)
    {
        var responseContent = await response.Content.ReadAsStringAsync();
        var flightSearchData = await DeserializeFlightSearchResponse(responseContent);

        return flightSearchData;
    }

    /// <summary>
    /// Asynchronously deserializes a JSON string into a FlightSearchOutputModel.
    /// </summary>
    /// <param name="flightResponse">The JSON string response from the flight search request.</param>
    /// <returns>A Task that represents the asynchronous operation, resulting in the deserialized FlightSearchOutputModel. Throws an exception if deserialization fails.</returns>
    /// <exception cref="Exception">Thrown when the JSON response cannot be deserialized into a FlightSearchOutputModel.</exception>
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