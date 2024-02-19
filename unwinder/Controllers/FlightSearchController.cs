using Microsoft.AspNetCore.Mvc;
using unwinder.Services;
using unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;
using unwinder.Services.AmadeusApiService.FlightSearch;
using unwinder.Services.AmadeusApiService.GetLocation;
using unwinder.Services.AmadeusApiService.GetCityIataCode;
using unwinder.Helpers;

namespace unwinder.Controllers;

/// <summary>
/// Controller responsible for handling flight search operations.
/// </summary>
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

    /// <summary>
    /// Retrieves a list of airports matching a given location.
    /// </summary>
    /// <param name="location">The location used to search for airports (e.g., city name).</param>
    /// <returns>A list of airport identifiers or a not found/error response.</returns>
    [HttpGet("api/flight-search/get-airport/{location}")]
    public async Task<ActionResult<IEnumerable<string>>> GetAirportLocation(string location)
    {
        try
        {
            var airports = await _getLocationService.GetLocation(location);

            if (!airports.Any())
            {
                return NotFound("No airports found for the given location.");
            }

            return Ok(airports);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "No data returned for location: {Location}", location);
            return NotFound("No airports found for the given location.");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error retrieving airport data for location: {Location}", location);
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Service unavailable.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting airport data for location: {Location}", location);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
        }
    }

    /// <summary>
    /// Retrieves a list of cities based on a given location keyword.
    /// </summary>
    /// <param name="location">The location used to search for cities (e.g., city name or airport code).</param>
    /// <returns>A list of city names with country names or a not found/error response.</returns>
    [HttpGet("api/flight-search/get-city/{location}")]
    public async Task<ActionResult<IEnumerable<string>>> GetCityLocation(string location)
    {
        try
        {
            var airports = await _getLocationService.GetLocation(location);

            if (!airports.Any())
            {
                return NotFound("No airports found for the given location.");
            }

            var cityNames = airports.Select(a => a.CityName + ", " + a.CountryName).Distinct().ToList();

            return Ok(cityNames);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "No data returned for location: {Location}", location);
            return NotFound("No cities found for the given location.");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error retrieving city data for location: {Location}", location);
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Service unavailable.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting city data for location: {Location}", location);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
        }
    }

    /// <summary>
    /// Conducts a flight search based on the provided search criteria.
    /// </summary>
    /// <param name="requestContent">The flight search request parameters.</param>
    /// <returns>A flight search result or an error response.</returns>
    [HttpPost("api/flight-search")]
    public async Task<ActionResult<string>> FlightSearch([FromBody] FlightSearchRequest requestContent)
    {
        List<string> numberOfTravelers = FlightSearchHelpers.RepeatString("ADULT", requestContent.NumberOfPassengers);
        string startDate = FlightSearchHelpers.ConvertIsoDateStringToDate(requestContent.When);
        string startTime = FlightSearchHelpers.ConvertIsoDateStringToTime(requestContent.When);
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
                .BuildDateTimeRange(startDate, startTime)
                .BuildOriginDestinations(origin, where)
                .BuildCurrencyCode("USD")
                .BuildDefaultValues()
                .Build();

            FlightSearchOutputModel flightSearchResult = await _flightSearchService.FlightSearch(requestParameters);

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