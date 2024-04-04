using Microsoft.AspNetCore.Mvc;
using unwidner.Models.AmadeusApiServiceModels.HotelSearchModels;
using unwinder.Controllers;
using unwinder.Services.AmadeusApiService.GetCityIataCode;
using unwinder.Services.AmadeusApiService.HotelSearch;

namespace unwinder.tests.Service.AmadeusApiService;

public class HotelSearchControllerTests
{
    private Mock<IHotelSearchListService> _mockHotelSearchListService;
    private Mock<IHotelSearchService> _mockHotelSearchService;
    private Mock<IGetCityIataCodeService> _mockgetCityIataCodeService;
    private HotelSearchController _controller;
    private Fixture _fixture;


    [SetUp]
    public void SetUp()
    {
        _mockHotelSearchListService = new Mock<IHotelSearchListService>();
        _mockHotelSearchService = new Mock<IHotelSearchService>();
        _mockgetCityIataCodeService = new Mock<IGetCityIataCodeService>();

        _controller = new HotelSearchController(
            _mockHotelSearchListService.Object,
            _mockHotelSearchService.Object,
            _mockgetCityIataCodeService.Object
        );

        _fixture = new Fixture();
    }

    [Test]
    public async Task HotelSearch_ReturnsOk_WithValidParameters()
    {
        var mockResponse = _fixture.Create<HotelSearchOutputModel>();
        var mockHotelListOutput = _fixture.Create<HotelSearchListOutputModel>();

        _mockgetCityIataCodeService.Setup(service => service.GetCityIataCode(It.IsAny<string>()))
                           .ReturnsAsync("MUC");

        _mockHotelSearchListService.Setup(service => service.SearchListOfHotels(It.IsAny<HotelSearchListParametersModel>()))
                                   .ReturnsAsync(mockHotelListOutput);

        _mockHotelSearchService.Setup(service => service.SearchHotel(It.IsAny<HotelSearchParametersModel>()))
                               .ReturnsAsync(mockResponse);

        var result = await _controller.HotelSearch(2, "2024-06-28", "2024-06-30", "Munich, Germany");

        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult.Value, Is.EqualTo(mockResponse));
    }

    [Test]
    public async Task HotelSearch_ReturnsNoContent_WhenNoHotelsFound()
    {
        var mockHotelListOutput = _fixture.Create<HotelSearchListOutputModel>();

        _mockgetCityIataCodeService.Setup(service => service.GetCityIataCode(It.IsAny<string>()))
                                   .ReturnsAsync("MUC");
        _mockHotelSearchListService.Setup(service => service.SearchListOfHotels(It.IsAny<HotelSearchListParametersModel>()))
                                   .ReturnsAsync(mockHotelListOutput);
        _mockHotelSearchService.Setup(service => service.SearchHotel(It.IsAny<HotelSearchParametersModel>()))
                               .ReturnsAsync(new HotelSearchOutputModel { Data = new List<Datum>() });

        var result = await _controller.HotelSearch(2, "2024-06-28", "2024-06-30", "City With No Hotels");

        Assert.IsInstanceOf<NoContentResult>(result.Result);
    }

    [Test]
    public async Task HotelSearch_ReturnsInternalServerError_WhenHotelSearchServiceFails()
    {
        _mockgetCityIataCodeService.Setup(service => service.GetCityIataCode(It.IsAny<string>()))
                                   .ReturnsAsync("MUC");
        _mockHotelSearchListService.Setup(service => service.SearchListOfHotels(It.IsAny<HotelSearchListParametersModel>()))
                                   .ReturnsAsync(new HotelSearchListOutputModel());
        _mockHotelSearchService.Setup(service => service.SearchHotel(It.IsAny<HotelSearchParametersModel>()))
                               .ThrowsAsync(new Exception("Internal Server Error"));

        var result = await _controller.HotelSearch(2, "2024-06-28", "2024-06-30", "Munich, Germany");

        // Assuming you catch exceptions and return StatusCode(500) for unhandled exceptions
        Assert.IsInstanceOf<ObjectResult>(result.Result);
        var objectResult = result.Result as ObjectResult;
        Assert.That(objectResult.StatusCode, Is.EqualTo(500));
    }
}