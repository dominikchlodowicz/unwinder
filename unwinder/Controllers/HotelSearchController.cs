using Microsoft.AspNetCore.Mvc;
using unwinder.Services;
using unwinder.Services.AmadeusApiService.HotelSearch;

namespace unwinder.Controllers;

[ApiController]
public class HotelSearchController : ControllerBase
{
    private readonly IHotelSearchListService _hotelSearchListService;
    private readonly IHotelSearchService _hotelSearchService;

    public HotelSearchController(
        IHotelSearchListService hotelSearchListService,
        IHotelSearchService hotelSearchService
    )
    {
        _hotelSearchListService = hotelSearchListService;
        _hotelSearchService = hotelSearchService;
    }

    [HttpPost("api/hotel-search")]
    public async Task<ActionResult<string>> HotelSearch([FromBody] HotelSearchRequest requestContent)
    {
        return Ok();
    }

    // Test controller method
    [HttpGet("api/hotel-search/test")]
    public async Task<ActionResult<string>> HotelSearchTest()
    {
        return Ok();
    }

}