using Microsoft.AspNetCore.Mvc;
using unwidner.Models.AmadeusApiServiceModels.HotelSearchModels;
using unwinder.Services;
using unwinder.Services.AmadeusApiService.GetCityIataCode;
using unwinder.Services.AmadeusApiService.HotelSearch;

namespace unwinder.Controllers;

[ApiController]
public class HotelSearchController : ControllerBase
{
    private readonly IHotelSearchListService _hotelSearchListService;
    private readonly IHotelSearchService _hotelSearchService;
    private readonly IGetCityIataCodeService _getCityIataCodeService;

    public HotelSearchController(
        IHotelSearchListService hotelSearchListService,
        IHotelSearchService hotelSearchService,
        IGetCityIataCodeService getCityIataCodeService
    )
    {
        _hotelSearchListService = hotelSearchListService;
        _hotelSearchService = hotelSearchService;
        _getCityIataCodeService = getCityIataCodeService;
    }

    // Test controller method
    [HttpGet("api/hotel-search/test")]
    public async Task<ActionResult<string>> HotelSearch(int adults, string checkIn, string checkOut, string cityCode)
    {
        string cityName = cityCode.Split(',')[0];

        var cityIataCode = await _getCityIataCodeService.GetCityIataCode(cityName);

        HotelSearchListParametersModel hotelSearchListParameters = new HotelSearchListParametersModel()
        {
            CityCode = cityIataCode,
            Radius = 5
        };

        var hotelSearchListOutput = await _hotelSearchListService.SearchListOfHotels(hotelSearchListParameters);


        HotelSearchParametersModel hotelSearchParameters = new HotelSearchParametersBuilder()
            .BuildHotelIds(hotelSearchListOutput)
            .BuildNumberOfAdults(adults)
            .BuildInOutDates(checkIn, checkOut)
            .BuildDefaultValues()
            .Build();

        HotelSearchOutputModel hotelSearchOutput = await _hotelSearchService.SearchHotel(hotelSearchParameters);

        return Ok(hotelSearchOutput);
    }
}