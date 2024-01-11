using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using unwinder.Services;
using System.Threading.Tasks;
using unwinder.Controllers;
using Microsoft.AspNetCore.Mvc;
using unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;
using System.Collections.Generic;
using unwinder.Services.AmadeusApiService.FlightSearch;
using unwinder.Services.AmadeusApiService.GetLocation;
using unwinder.Services.AmadeusApiService.GetCityIataCode;
using Newtonsoft.Json;
using unwinder.Models.AmadeusApiServiceModels.GetLocationModels;
using FluentAssertions;
using System.Net;
using NUnit.Framework.Constraints;
using System.Diagnostics;


namespace unwinder.tests.Service.AmadeusApiService;

public class FlightSearchControllerTests
{
    private Mock<IFlightSearchService> _mockFlightSearchService;
    private Mock<IGetLocationService> _mockGetLocationService;
    private Mock<IGetCityIataCodeService> _mockGetCityIataService;
    private Mock<ILogger<FlightSearchController>> _mockLogger;
    private Mock<IHttpClientFactory> _mockHttpClientFactory;
    private Mock<IGetToken> _mockBearerToken;
    private FlightSearchController _controller;
    private Fixture _fixture;

    [SetUp]
    public void SetUp()
    {
        _mockFlightSearchService = new Mock<IFlightSearchService>();
        _mockGetLocationService = new Mock<IGetLocationService>();
        _mockGetCityIataService = new Mock<IGetCityIataCodeService>();
        _mockLogger = new Mock<ILogger<FlightSearchController>>();
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        _mockBearerToken = new Mock<IGetToken>();

        _controller = new FlightSearchController(
            _mockFlightSearchService.Object,
            _mockGetLocationService.Object,
            _mockGetCityIataService.Object,
            _mockLogger.Object,
            _mockHttpClientFactory.Object,
            _mockBearerToken.Object
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
        FlightSearchParameters testRequestParameters = new FlightSearchParametersBuilder()
            .BuildNumberOfTravelers(new List<string> { "Adult" })
            .BuildDateTimeRange("2024-01-24", "00:00:00")
            .BuildOriginDestinations("Munich", "Madrid")
            .BuildCurrencyCode("USD")
            .BuildDefaultValues()
            .Build();

        var expectedResponse = _fixture.Create<FlightSearchOutputModel>();

        _mockFlightSearchService.Setup(s => s.FlightSearch(testRequestParameters)).ReturnsAsync(expectedResponse);

        var actionResult = await _controller.FlightSearch(new FlightSearchRequest());

        Assert.IsInstanceOf<OkObjectResult>(actionResult.Result);

        var okResult = actionResult.Result as OkObjectResult;
        Assert.IsNotNull(okResult);

        var responseValue = okResult.Value;
        Assert.That(responseValue, Is.EqualTo(expectedResponse));
    }

    [Test]
    public async Task FlightSearch_Returns500_WhenParametersAreInvalid()
    {
        throw new NotImplementedException();
    }

    [Test]
    public async Task FlightSearch_Returns500_WhenGeneralException()
    {
        throw new NotImplementedException();
    }
}