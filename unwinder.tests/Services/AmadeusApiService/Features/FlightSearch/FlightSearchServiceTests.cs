using System.Net;
using Moq.Protected;
using unwinder.Services;
using unwinder.Services.AmadeusApiService;
using unwinder.tests.Services.Helpers;
using unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;
using Newtonsoft.Json;
using unwinder.Services.AmadeusApiService.FlightSearch;
using FluentAssertions;
using System.Text.RegularExpressions;

namespace unwinder.tests.Service.AmadeusApiService;

public class FlightSearchServiceTests
{
    private Mock<IGetToken> _getTokenMock;
    private Fixture _fixture;
    private FlightSearchParametersBuilder _defaultParametersBuilder;

    [SetUp]
    public void Setup()
    {
        _getTokenMock = new Mock<IGetToken>();
        _getTokenMock.Setup(_ => _.GetAuthToken()).ReturnsAsync("test_token");
        _fixture = new Fixture();
        _defaultParametersBuilder = AmadeusApiHttpClientTestHelper.CreateDefaultFlightSearchParametersBuilder();
    }

    [Test]
    public void FlightSearch_WithValidParameters_ReturnsFlightSearchOutputModel()
    {
        var expectedFlights = _fixture.Create<FlightSearchOutputModel>();
        var httpResponseJson = JsonConvert.SerializeObject(expectedFlights);
        var httpClientMock = AmadeusApiHttpClientTestHelper.SetupHttpClient(HttpStatusCode.OK, httpResponseJson);
        var sut = new FlightSearchService(httpClientMock, _getTokenMock.Object);
        var sutParameters = _defaultParametersBuilder.Build();

        Task<FlightSearchOutputModel> result = sut.FlightSearch(sutParameters);

        result.Result.Should().BeEquivalentTo(expectedFlights);
    }

    [Test]
    [TestCase(HttpStatusCode.InternalServerError)]
    [TestCase(HttpStatusCode.NotFound)]
    [TestCase(HttpStatusCode.BadGateway)]
    public void FlightSearch_ThrowsHttpRequestException_WhenApiResponseIsInvalid(HttpStatusCode statusCode)
    {
        var expectedFlights = _fixture.Create<FlightSearchOutputModel>();
        var httpResponseJson = JsonConvert.SerializeObject(expectedFlights);
        var httpClientMock = AmadeusApiHttpClientTestHelper.SetupHttpClient(statusCode, httpResponseJson);
        var sut = new FlightSearchService(httpClientMock, _getTokenMock.Object);
        var sutParameters = _defaultParametersBuilder.Build();

        Assert.ThrowsAsync<HttpRequestException>(async () => await sut.FlightSearch(sutParameters));
    }

    [Test]
    public void FlightSearch_ThrowsHttpRequestException_WhenTokenRetrievalFails()
    {
        _getTokenMock.Setup(_ => _.GetAuthToken()).ThrowsAsync(new HttpRequestException("Token retrieval failed"));
        var expectedFlights = _fixture.Create<FlightSearchOutputModel>();
        var httpResponseJson = JsonConvert.SerializeObject(expectedFlights);
        var httpClientMock = AmadeusApiHttpClientTestHelper.SetupHttpClient(HttpStatusCode.OK, httpResponseJson);
        var sut = new FlightSearchService(httpClientMock, _getTokenMock.Object);
        var sutParameters = _defaultParametersBuilder.Build();

        Assert.ThrowsAsync<HttpRequestException>(() => sut.FlightSearch(sutParameters));
    }

    [Test]
    public void FlightSearch_ThrowsJsonReaderException_WhenDataIsNotCompatibleDuringDeserialization()
    {
        var expectedFlights = _fixture.Create<FlightSearchOutputModel>();
        var httpResponseJson = JsonConvert.SerializeObject(expectedFlights);
        var wrongCountJson = Regex.Replace(httpResponseJson, "\"count\":\\d+", "\"count\":\"not_an_integer\"");
        var httpClientMock = AmadeusApiHttpClientTestHelper.SetupHttpClient(HttpStatusCode.OK, wrongCountJson);
        var sut = new FlightSearchService(httpClientMock, _getTokenMock.Object);
        var sutParameters = _defaultParametersBuilder.Build();

        Assert.ThrowsAsync<JsonReaderException>(() => sut.FlightSearch(sutParameters));
    }
}