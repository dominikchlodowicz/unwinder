using Microsoft.AspNetCore.Mvc;
using unwinder.Services;
using unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;

namespace unwinder.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlightSearchController : ControllerBase 
{

    private readonly IAmadeusApiService _amadeusApiService;

    public FlightSearchController (IAmadeusApiService amadeusApiService)
    {
        _amadeusApiService = amadeusApiService;
    }


    [HttpGet]
    public async Task<string> GetLocation()
    {
        var airports = await _amadeusApiService.GetLocation("Paris");
        return airports;
    }

    [HttpGet]
    public async Task<FlightSearchOutputModel> FlightSearch()
    {
        var flightSearchParameters = new FlightSearchParameters
        {
            OriginLocationCode = "LAX",
            DestinationLocationCode = "JFK",
            DepartureDate = DateTime.Now.AddDays(14),
            Adults = 1,
            Max = 10
        };
        var flightOffers = await _amadeusApiService.FlightSearch(flightSearchParameters);
        return flightOffers;
    }

}