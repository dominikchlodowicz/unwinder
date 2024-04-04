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
        // Arrange
        // var mockResponse = _fixture.Create<HotelSearchOutputModel>();
        // _mockgetCityIataCodeService.Setup(service => service.GetCityIataCode(It.IsAny<string>())).ReturnsAsync("MUC");
        // _mockHotelSearchListService.Setup(service => service.SearchListOfHotels(It.IsAny<HotelSearchListParametersModel>())).ReturnsAsync(new HotelSearchListOutputModel());
        // _mockHotelSearchService.Setup(service => service.SearchHotel(It.IsAny<HotelSearchParametersModel>())).ReturnsAsync(mockResponse);

        // // Act
        // var result = await _controller.HotelSearch(2, "2024-06-28", "2024-06-30", "Munich, Germany");

        // // Assert
        // Assert.IsInstanceOf<OkObjectResult>(result.Result);
        // var okResult = result.Result as OkObjectResult;
        // Assert.AreEqual(mockResponse, okResult.Value);
    }

    [Test]
    public async Task HotelSearch_ReturnsBadRequest_WhenCityCodeServiceFails()
    {
        // Arrange
        // _mockgetCityIataCodeService.Setup(service => service.GetCityIataCode(It.IsAny<string>())).ThrowsAsync(new Exception("Service error"));

        // // Act
        // var result = await _controller.HotelSearch(1, "2024-01-01", "2024-01-05", "Nonexistent City");

        // // Assert
        // Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        // var badRequestResult = result.Result as BadRequestObjectResult;
        // Assert.AreEqual("Service error", badRequestResult.Value);
    }


    [Test]
    public async Task HotelSearch_ReturnsNoContent_WhenNoHotelsFound()
    {
        // // Arrange
        // _mockgetCityIataCodeService.Setup(service => service.GetCityIataCode(It.IsAny<string>())).ReturnsAsync("MUC");
        // _mockHotelSearchListService.Setup(service => service.SearchListOfHotels(It.IsAny<HotelSearchListParametersModel>())).ReturnsAsync(new HotelSearchListOutputModel { Data = new List<Datum>() });
        // _mockHotelSearchService.Setup(service => service.SearchHotel(It.IsAny<HotelSearchParametersModel>())).ReturnsAsync(new HotelSearchOutputModel { Data = new List<Datum>() });

        // // Act
        // var result = await _controller.HotelSearch(2, "2024-06-28", "2024-06-30", "Munich, Germany");

        // // Assert
        // Assert.IsInstanceOf<NoContentResult>(result.Result);
    }


}