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
        try
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

            if (hotelSearchOutput == null || hotelSearchOutput.Data.Count == 0)
            {
                return NoContent();
            }

            return Ok(hotelSearchOutput);
        }
        catch (Exception ex) when (ex is ArgumentException || ex is FormatException)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An internal error occurred");
        }
    }
}