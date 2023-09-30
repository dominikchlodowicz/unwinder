using Microsoft.AspNetCore.Mvc;
using unwinder.Services;
using unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using System.Diagnostics;

namespace unwinder.Controllers;

[ApiController]
public class FlightSearchController : ControllerBase
{

    private readonly IAmadeusApiService _amadeusApiService;
    private readonly ILogger<FlightSearchController> _logger;
    private readonly HttpClient _httpClient;
    private readonly IGetToken _bearerToken;


    public FlightSearchController(IAmadeusApiService amadeusApiService,
         ILogger<FlightSearchController> logger,
         IHttpClientFactory httpClientFactory,
         IGetToken bearerToken)
    {
        _amadeusApiService = amadeusApiService;
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient("AmadeusApiV2");
        _bearerToken = bearerToken;
    }


    [HttpGet("api/getlocation")]
    public async Task<string> GetLocation()
    {
        var airports = await _amadeusApiService.GetLocation("Paris");
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
                    Id = "1",
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
                            Cabin = "BUSINESS",
                            Coverage = "MOST_SEGMENTS",
                            OriginDestinationIds = new List<string> { "1" }
                        }
                    }
                }
            }
        };

        var flightSearchResult = await _amadeusApiService.FlightSearch(requestParameters);
        var serializedflighSearchResult = JsonConvert.SerializeObject(flightSearchResult);
        return serializedflighSearchResult;
    }
}