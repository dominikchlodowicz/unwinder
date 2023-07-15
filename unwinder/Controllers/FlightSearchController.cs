using Microsoft.AspNetCore.Mvc;
using unwinder.Services;

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
    public async Task<>
}