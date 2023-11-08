using System.Net;
using Moq.Protected;
using unwinder.Services;
using unwinder.Services.AmadeusApiService;
using unwinder.tests.Services.Helpers;
using unwinder.Models.AmadeusApiServiceModels.GetLocationModels;
using Newtonsoft.Json;
using unwinder.Services.AmadeusApiService.GetLocation;

namespace unwinder.tests.Services.AmadeusApiService;

public class GetLocationServiceTests
{
    private Mock<IGetToken> _getTokenMock;
    private Fixture _fixture;

    [SetUp]
    public void Setup()
    {
        _getTokenMock = new Mock<IGetToken>();
        _getTokenMock.Setup(_ => _.GetAuthToken()).ReturnsAsync("test_token");
        _fixture = new Fixture();
    }

    [Test]
    public async Task GetLocationService_ReturnsLocation_WhenEverythingIsRight()
    {
        // Mock output variables
        var expectedLocations = _fixture.Create<GetLocationAirportResponseModel>();
        // var wrapper = new GetLocationAirportModelWrapper { data = expectedLocations };
        var httpResponseJson = JsonConvert.SerializeObject(expectedLocations);

        var httpClientMock = AmadeusApiHttpClientTestHelper.SetupHttpClient(HttpStatusCode.OK, httpResponseJson);
        var sut = new GetLocationService(httpClientMock, _getTokenMock.Object);

        // Act
        var result = await sut.GetLocation("test");

        Assert.That(expectedLocations.data[0].name, Is.EqualTo(result.FirstOrDefault().Name));
        Assert.That(expectedLocations.data[0].iataCode, Is.EqualTo(result.FirstOrDefault().IataCode));
        Assert.That(expectedLocations.data[0].address.cityName, Is.EqualTo(result.FirstOrDefault().CityName));
    }

    [Test]
    [TestCase(HttpStatusCode.InternalServerError)]
    [TestCase(HttpStatusCode.NotFound)]
    [TestCase(HttpStatusCode.BadGateway)]
    public void GetLocationService_ThrowsHttoRequestException_WhenApiResponseIsInvalid(HttpStatusCode statusCode)
    {
        var httpClientMock = AmadeusApiHttpClientTestHelper.SetupHttpClient(statusCode);
        var sut = new GetLocationService(httpClientMock, _getTokenMock.Object);

        Assert.ThrowsAsync<HttpRequestException>(async () => await sut.GetLocation("test"));
    }

    public void GetLocationService_ThrowsException_WhenResponseIsEmpty(string expectedToken)
    {
        var expectedLocations = _fixture.Create<GetLocationAirportResponseModel>();
        expectedLocations.data = null; 
        var httpResponseJson = JsonConvert.SerializeObject(expectedLocations);
        var httpClientMock = AmadeusApiHttpClientTestHelper.SetupHttpClient(HttpStatusCode.OK, httpResponseJson);
        var sut = new GetLocationService(httpClientMock, _getTokenMock.Object);

        Assert.ThrowsAsync<InvalidOperationException>(async () => await sut.GetLocation("test"));
    }

    [Test]
    public void GetLocationService_ThrowsException_WhenJsonStructureIsUnexpected()
    {
        var expectedLocations = _fixture.Create<GetLocationAirportResponseModel>();
        expectedLocations.data[0].name = null;
        var httpResponseJson = JsonConvert.SerializeObject(expectedLocations);

        var httpClientMock = AmadeusApiHttpClientTestHelper.SetupHttpClient(HttpStatusCode.OK, httpResponseJson);
        var sut = new GetLocationService(httpClientMock, _getTokenMock.Object);

        Assert.ThrowsAsync<InvalidOperationException>(async () => await sut.GetLocation("test"));
    }
}



