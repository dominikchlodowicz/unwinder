using Microsoft.Extensions.Logging;
using unwinder.Services;
using unwinder.Controllers;
using Microsoft.AspNetCore.Mvc;
using unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;
using unwinder.Services.AmadeusApiService.FlightSearch;
using unwinder.Services.AmadeusApiService.GetLocation;
using unwinder.Services.AmadeusApiService.GetCityIataCode;
using unwinder.Models.AmadeusApiServiceModels.GetLocationModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;

namespace unwinder.tests.Service.AmadeusApiService;

public class FlightSearchControllerTests
{
    private Mock<IFlightSearchService> _mockFlightSearchService;
    private Mock<IGetLocationService> _mockGetLocationService;
    private Mock<IGetCityIataCodeService> _mockGetCityIataService;
    private Mock<ILogger<FlightSearchController>> _mockLogger;
    private FlightSearchController _controller;
    private Fixture _fixture;

    [SetUp]
    public void SetUp()
    {
        _mockFlightSearchService = new Mock<IFlightSearchService>();
        _mockGetLocationService = new Mock<IGetLocationService>();
        _mockGetCityIataService = new Mock<IGetCityIataCodeService>();
        _mockLogger = new Mock<ILogger<FlightSearchController>>();

        _controller = new FlightSearchController(
            _mockFlightSearchService.Object,
            _mockGetLocationService.Object,
            _mockGetCityIataService.Object,
            _mockLogger.Object
        );

        _fixture = new Fixture();
    }

    [Test]
    public async Task GetAirportLocation_WithInvalidLocation_ReturnsNotFoundObjectResult()
    {
        string testLocation = "Bielawa";

        _mockGetLocationService.Setup(s => s.GetLocation(testLocation))
            .ReturnsAsync(new List<GetLocationAirportModel>());

        var result = await _controller.GetAirportLocation(testLocation);

        Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
    }

    [Test]
    public async Task GetAirportLocation_WithValidLocation_ReturnsOkObjectResult()
    {
        string testLocation = "Paris";
        List<GetLocationAirportModel> expectedReturnedAirports = _fixture.Build<GetLocationAirportModel>()
            .With(x => x.Name, testLocation)
            .CreateMany(3)
            .ToList();

        _mockGetLocationService.Setup(s => s.GetLocation(testLocation)).ReturnsAsync(expectedReturnedAirports);

        var actionResult = await _controller.GetAirportLocation(testLocation);

        Assert.IsInstanceOf<OkObjectResult>(actionResult.Result);

        var okResult = actionResult.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        var actualAirports = okResult.Value as IEnumerable<GetLocationAirportModel>;
        actualAirports.Should().BeEquivalentTo(expectedReturnedAirports);
    }

    [Test]
    public async Task GetCityLocation_WithInvalidCity_ReturnsNotFoundObjectResult()
    {
        string testLocation = "Bielawa";

        _mockGetLocationService.Setup(s => s.GetLocation(testLocation))
            .ReturnsAsync(new List<GetLocationAirportModel>());

        var result = await _controller.GetCityLocation(testLocation);

        Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
    }

    [Test]
    public async Task GetCityLocation_WithValidCity_ReturnsOkObjectResult()
    {
        string testLocation = "Paris";
        List<GetLocationAirportModel> expectedReturnedAirports = _fixture.Build<GetLocationAirportModel>()
            .With(x => x.Name, testLocation)
            .CreateMany(3)
            .ToList();

        var processedExpectedReturnedCities = expectedReturnedAirports.Select(a => a.CityName + ", " + a.CountryName).Distinct().ToList();

        _mockGetLocationService.Setup(s => s.GetLocation(testLocation)).ReturnsAsync(expectedReturnedAirports);

        var actionResult = await _controller.GetCityLocation(testLocation);

        Assert.IsInstanceOf<OkObjectResult>(actionResult.Result);

        var okResult = actionResult.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        var actualCities = okResult.Value as IEnumerable<string>;
        actualCities.Should().BeEquivalentTo(processedExpectedReturnedCities);
    }

    [Test]
    public async Task FlightSearch_ReturnsOk_WhenServiceReturnsValidData()
    {
        var expectedResponse = _fixture.Create<FlightSearchOutputModel>();

        var flightSearchRequest = new FlightSearchRequest
        {
            NumberOfPassengers = 2,
            When = DateTime.Now.Date.AddDays(1).ToString("yyyy-MM-dd"),
            Back = DateTime.Now.Date.AddDays(2).ToString("yyyy-MM-dd"),
            Where = "Madrid",
            Origin = "Munich"
        };

        _mockFlightSearchService.Setup(s => s.FlightSearch(It.IsAny<FlightSearchParameters>()))
                                .ReturnsAsync(expectedResponse);

        var actionResult = await _controller.FlightSearch(flightSearchRequest);

        Assert.IsInstanceOf<ObjectResult>(actionResult.Result);
        var result = actionResult.Result as ObjectResult;
        Assert.IsNotNull(result);
        Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status200OK));

        var responseValue = result.Value;
        Assert.That(responseValue, Is.EqualTo(expectedResponse));
    }

    [Test]
    public async Task FlightSearch_Returns500_WhenParametersAreInvalid()
    {
        var expectedResponse = _fixture.Create<FlightSearchOutputModel>();

        var flightSearchRequest = new FlightSearchRequest
        {
            NumberOfPassengers = 2,
            When = "11-01-2021", // bad format and past
            Back = DateTime.Now.Date.AddDays(2).ToString("yyyy-MM-dd"),
            Where = "Madrid",
            Origin = "Munich"
        };

        _mockFlightSearchService.Setup(s => s.FlightSearch(It.IsAny<FlightSearchParameters>()))
                        .ReturnsAsync(expectedResponse);

        var actionResult = await _controller.FlightSearch(flightSearchRequest);

        Assert.IsInstanceOf<ObjectResult>(actionResult.Result);
        var result = actionResult.Result as ObjectResult;

        Assert.IsNotNull(result);
        Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
    }
}