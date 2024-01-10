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
using unwinder.Services.AmadeusApiService.GetCityIataCode;


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
    //TODO: fix FlightSearchControllerTests

    [Ignore("Need to rebuild these controller tests, due to decoupling them to different methods")]
    [Test]
    public async Task GGetAirportLocation_WithExistingLocation_ReturnsSerializedAirports()
    {
        string testLocation = "New York City";
        List<GetLocationAirportModel> expectedReturnedAirports = _fixture.Build<GetLocationAirportModel>()
                                                            .With(x => x.Name, testLocation)
                                                            .CreateMany(3)
                                                            .ToList();

        _mockGetLocationService.Setup(s => s.GetLocation(testLocation)).ReturnsAsync(expectedReturnedAirports);

        var result = await _controller.GetAirportLocation(testLocation);

        Assert.IsInstanceOf<string>(result);
        // var deserializedResult = JsonConvert.DeserializeObject<List<GetLocationAirportModel>>(result);
        // deserializedResult.Should().BeEquivalentTo(expectedReturnedAirports);
    }

    [Ignore("Need to rebuild these controller tests, due to decoupling them to different methods")]
    [Test]
    public void GetLocation_WithInvalidLocaton_ThrowsInvalidOperationException()
    {
        string testLocation = "Bielawa";
        List<GetLocationAirportModel> expectedReturnedAirports = new List<GetLocationAirportModel> { };

        _mockGetLocationService.Setup(s => s.GetLocation(testLocation)).ReturnsAsync(expectedReturnedAirports);

        // Assert.ThrowsAsync<InvalidOperationException>(async () => await _controller.GetLocation(testLocation));
    }
}