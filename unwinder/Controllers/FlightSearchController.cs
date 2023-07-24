using Microsoft.AspNetCore.Mvc;
using unwinder.Services;
using unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;

namespace unwinder.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlightSearchController : ControllerBase
{

    private readonly IAmadeusApiService _amadeusApiService;
    private readonly ILogger<FlightSearchController> _logger;

    public FlightSearchController(IAmadeusApiService amadeusApiService, ILogger<FlightSearchController> logger)
    {
        _amadeusApiService = amadeusApiService;
        _logger = logger;
    }


    // [HttpGet]
    // public async Task<string> GetLocation()
    // {
    //     var airports = await _amadeusApiService.GetLocation("Paris");
    //     return airports;
    // }

    [HttpGet]
    public async void FlightSearch()
    {
        var flightSearchParameters = new FlightSearchParameters
        {
            OriginLocationCode = "LAX",
            DestinationLocationCode = "JFK",
            DepartureDate = DateTime.Today.AddDays(14).ToString("yyyy-MM-dd"),
            Adults = 1,
            Max = 10
        };

        var flightOffers = await _amadeusApiService.FlightSearch(flightSearchParameters);



        // TODO: deserialization returns nulls
        foreach (var flight in flightOffers)
        {   
            _logger.LogInformation("This is flight object: {Price count}", flight.FlightSearchOutput);
        }

    }
}