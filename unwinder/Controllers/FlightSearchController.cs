using Microsoft.AspNetCore.Mvc;
using unwinder.Services;
using unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace unwinder.Controllers;

[ApiController]
[Route("api/[controller]")]
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


    // [HttpGet]
    // public async Task<string> GetLocation()
    // {
    //     var airports = await _amadeusApiService.GetLocation("Paris");
    //     return airports;
    // }

    // [HttpGet]
    // public async void FlightSearch()
    // {
    //     var flightSearchParameters = new FlightSearchParameters
    //     {
    //         OriginLocationCode = "LAX",
    //         DestinationLocationCode = "JFK",
    //         DepartureDate = DateTime.Today.AddDays(14).ToString("yyyy-MM-dd"),
    //         Adults = 1,
    //         Max = 10
    //     };

    //     var flightOffers = await _amadeusApiService.FlightSearch(flightSearchParameters);



    //     // TODO: deserialization returns nulls
    //     foreach (var flight in flightOffers)
    //     {
    //         _logger.LogInformation("This is flight object: {Price count}", flight.FlightSearchOutput);
    //     }

    // }

    [HttpGet]
    public async Task<IActionResult> PostFlightSearch()
    {
        var token = await _bearerToken.GetAuthToken();

        var request = new FlightOfferRequest
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

        var json = JsonConvert.SerializeObject(request);

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync("shopping/flight-offers", content);

        var responseString = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            // _logger.LogInformation("This is response from post action: {response}", response);
            var deserializedRepsonse = JsonConvert.DeserializeObject<FlightOfferResponse>(responseString);
            _logger.LogInformation("This is response from post action meta.count: {response}", deserializedRepsonse.Meta.Count);
            return Ok(result);
        }
        else
        {
            var log = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Error calling the third-party API: {log}",  log);
            return StatusCode((int)response.StatusCode, "Error calling the third-party API");
        }
    }
}