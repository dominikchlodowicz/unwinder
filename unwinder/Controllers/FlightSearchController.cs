using Microsoft.AspNetCore.Mvc;
using unwinder.Services;
using unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using System.Diagnostics;
using unwinder.Services.AmadeusApiService.FlightSearch;
using unwinder.Services.AmadeusApiService.GetLocation;
using FluentAssertions;


namespace unwinder.Controllers;

[ApiController]
public class FlightSearchController : ControllerBase
{
    private readonly IFlightSearchService _flightSearchService;
    private readonly IGetLocationService _getLocationService;
    private readonly ILogger<FlightSearchController> _logger;
    private readonly HttpClient _httpClient;
    private readonly IGetToken _bearerToken;


    public FlightSearchController(
        IFlightSearchService flightSearchService,
        IGetLocationService getLocationService,
        ILogger<FlightSearchController> logger,
        IHttpClientFactory httpClientFactory,
        IGetToken bearerToken)
    {
        _flightSearchService = flightSearchService;
        _getLocationService = getLocationService;
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient("AmadeusApiV2");
        _bearerToken = bearerToken;
    }

    [HttpGet("api/flight-search/get-airport/{location}")]
    public async Task<ActionResult<IEnumerable<string>>> GetAirportLocation(string location)
    {
        var airports = await _getLocationService.GetLocation(location);

        if (airports == null || !airports.Any())
        {
            return NotFound("No airports found for the given location.");
        }

        return Ok(airports);
    }

    [HttpGet("api/flight-search/get-city/{location}")]
    public async Task<ActionResult<IEnumerable<string>>> GetCityLocation(string location)
    {
        var airports = await _getLocationService.GetLocation(location);

        if (airports == null || !airports.Any())
        {
            return NotFound("No airports found for the given location.");
        }

        var cityNames = airports.Select(a => a.CityName + ", " + a.CountryName).Distinct().ToList();

        return Ok(cityNames);
    }

    [HttpGet("api/flight-search")]
    public async Task<ActionResult<string>> FlightSearch()
    {
        FlightSearchParameters requestParameters = new FlightSearchParametersBuilder()
            .BuildNumberOfTravelers(new List<string> { "ADULT" })
            .BuildDateTimeRange("2023-11-01", "10:00:00")
            .BuildOriginDestinations("NYC", "MAD")
            .BuildCurrencyCode("USD")
            .BuildDefaultValues()
            .Build();

        var flightSearchResult = await _flightSearchService.FlightSearch(requestParameters);

        if (flightSearchResult == null)
        {
            return NotFound("Flight search did not return any results.");
        }

        return Ok(flightSearchResult);
    }
}