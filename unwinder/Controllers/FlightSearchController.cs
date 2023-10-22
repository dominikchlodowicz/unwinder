using Microsoft.AspNetCore.Mvc;
using unwinder.Services;
using unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using System.Diagnostics;
using unwinder.Services.AmadeusApiService.FlightSearch;
using unwinder.Services.AmadeusApiService.GetLocation;


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


    [HttpGet("api/getlocation/{location}")]
    public async Task<string> GetLocation(string location)
    {
        var airports = await _getLocationService.GetLocation(location);
        var serializedAirports = JsonConvert.SerializeObject(airports);
        return serializedAirports;
    }

    [HttpGet("api/flightsearch")]
    public async Task<string> FlightSearch()
    {
        var requestParameters = new FlightSearchParameters
        {
            CurrencyCode = "USD",
            OriginDestinations = new List<OriginDestination>
            {
                new OriginDestination
                {
                    Id = "1",
                    OriginLocationCode = "NYC",
                    DestinationLocationCode = "MAD",
                    DepartureDateTimeRange = new DepartureDateTimeRange
                    {
                        Date = "2023-11-01",
                        Time = "10:00:00"
                    }
                }
            },
            Travelers = new List<Traveler>
            {
                new Traveler
                {
                    Id = "2",
                    TravelerType = "ADULT"
                }
            },
            Sources = new List<string> { "GDS" },
            SearchCriteria = new SearchCriteria
            {
                MaxFlightOffers = 2,
                FlightFilters = new FlightFilters
                {
                    CabinRestrictions = new List<CabinRestriction>
                    {
                        new CabinRestriction
                        {
                            Cabin = "ECONOMY",
                            Coverage = "MOST_SEGMENTS",
                            OriginDestinationIds = new List<string> { "1" }
                        }
                    }
                }
            }
        };

        var flightSearchResult = await _flightSearchService.FlightSearch(requestParameters);
        var serializedflighSearchResult = JsonConvert.SerializeObject(flightSearchResult);
        return serializedflighSearchResult;
    }
}