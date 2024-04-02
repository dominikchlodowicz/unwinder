using Microsoft.AspNetCore.Mvc;
using unwidner.Models.AmadeusApiServiceModels.HotelSearchModels;
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

    //TODO: Complete this controller
    [HttpPost("api/hotel-search")]
    public async Task<ActionResult<string>> HotelSearch([FromBody] HotelSearchRequest requestContent)
    {
        return Ok();
    }

    // Test controller method
    [HttpGet("api/hotel-search/test")]
    public async Task<ActionResult<string>> HotelSearchTest()
    {
        HotelSearchListParametersModel hotelSearchListParameters = new HotelSearchListParametersModel()
        {
            CityCode = "DEL",
            Radius = 5
        };

        var hotelSearchListOutput = await _hotelSearchListService.SearchListOfHotels(hotelSearchListParameters);


        HotelSearchParametersModel hotelSearchParameters = new HotelSearchParametersBuilder()
            .BuildHotelIds(hotelSearchListOutput)
            .BuildNumberOfAdults(2)
            .BuildInOutDates("2024-04-15", "2024-04-19")
            .BuildDefaultValues()
            .Build();

        HotelSearchOutputModel hotelSearchOutput = await _hotelSearchService.SearchHotel(hotelSearchParameters);

        return Ok(hotelSearchOutput);
    }
}
