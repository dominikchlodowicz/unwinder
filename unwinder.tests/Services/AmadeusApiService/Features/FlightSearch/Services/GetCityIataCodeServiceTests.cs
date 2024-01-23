using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using unwinder.Services.AmadeusApiService.GetLocation;
using unwinder.Services.AmadeusApiService.GetCityIataCode;
using AutoFixture;
using unwinder.Models.AmadeusApiServiceModels.GetLocationModels;

[TestFixture]
public class GetCityIataCodeServiceTests
{
    private Mock<IGetLocationService> _mockGetLocationService;
    private GetCityIataCodeService _getCityIataCodeService;
    private Fixture _fixture;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();
        _mockGetLocationService = new Mock<IGetLocationService>();
        _getCityIataCodeService = new GetCityIataCodeService(_mockGetLocationService.Object);
    }

    [Test]
    public async Task GetCityIataCode_WithValidKeyword_ReturnsCityCode()
    {
        var keyword = "someCity";
        var airports = _fixture.CreateMany<GetLocationAirportModel>(3).ToList();
        _mockGetLocationService.Setup(s => s.GetLocation(keyword)).ReturnsAsync(airports);

        var result = await _getCityIataCodeService.GetCityIataCode(keyword);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(airports.First().CityCode));
    }

    [Test]
    public void GetCityIataCode_WithInvalidKeyword_ThrowsException()
    {
        var keyword = "someCity";
        var airports = new List<GetLocationAirportModel>();
        _mockGetLocationService.Setup(s => s.GetLocation(keyword)).ReturnsAsync(airports);

        var ex = Assert.ThrowsAsync<Exception>(async () =>
            await _getCityIataCodeService.GetCityIataCode(keyword));

        Assert.That(ex.Message, Is.EqualTo("No airports found for the given location."));
    }
}
