using Microsoft.AspNetCore.Mvc;
using unwinder.Services;
using unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using System.Diagnostics;
using unwinder.Services.AmadeusApiService.FlightSearch;
using unwinder.Services.AmadeusApiService.GetLocation;
using unwinder.Services.AmadeusApiService.GetCityIataCode;
using unwinder.Helpers;
using FluentAssertions;


namespace unwinder.Controllers;

[ApiController]
public class FlightSearchController : ControllerBase
{
    private readonly IFlightSearchService _flightSearchService;
    private readonly IGetLocationService _getLocationService;
    private readonly IGetCityIataCodeService _getCityIataCodeService;
    private readonly ILogger<FlightSearchController> _logger;
    private readonly HttpClient _httpClient;
    private readonly IGetToken _bearerToken;


    public FlightSearchController(
        IFlightSearchService flightSearchService,
        IGetLocationService getLocationService,
        IGetCityIataCodeService getCityIataCodeService,
        ILogger<FlightSearchController> logger,
        IHttpClientFactory httpClientFactory,
        IGetToken bearerToken)
    {
        _flightSearchService = flightSearchService;
        _getLocationService = getLocationService;
        _getCityIataCodeService = getCityIataCodeService;
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

    /// <summary>
    /// Gets city with airport
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
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

    [HttpPost("api/flight-search")]
    public async Task<ActionResult<string>> FlightSearch([FromBody] FlightSearchRequest requestContent)
    {
        List<string> numberOfTravelers = FlightSearchHelpers.RepeatString("ADULT", requestContent.NumberOfPassengers);
        string startDate = FlightSearchHelpers.ConvertIsoDateStringToDate(requestContent.When);
        string endDate = FlightSearchHelpers.ConvertIsoDateStringToDate(requestContent.Back);
        string destinationCity = requestContent.Where.Split(',')[0];
        string originCity = requestContent.Origin.Split(',')[0];
        string where = await _getCityIataCodeService.GetCityIataCode(destinationCity);
        string origin = await _getCityIataCodeService.GetCityIataCode(originCity);

        // Amadeus API anti DDOS protection workaround
        System.Threading.Thread.Sleep(2000);

        try
        {
            FlightSearchParameters requestParameters = new FlightSearchParametersBuilder()
                .BuildNumberOfTravelers(numberOfTravelers)
                .BuildDateTimeRange(startDate, "00:00:00")
                .BuildOriginDestinations(origin, where)
                .BuildCurrencyCode("USD")
                .BuildDefaultValues()
                .Build();

            FlightSearchOutputModel flightSearchResult = await _flightSearchService.FlightSearch(requestParameters);
            flightSearchResult.FlightBackData ??= new FlightBack();
            flightSearchResult.FlightBackData.FlightBackDate = endDate;

            return Ok(flightSearchResult);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ErrorMessages.FlightSearchReturnedException);
            return StatusCode(500, ErrorMessages.FlightSearchApiHttpError);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ErrorMessages.FlightSearchReturnedException);
            return StatusCode(500, ErrorMessages.FlightSearchNotFound);
        }
    }
}