using System.Net;
using System.Net.Http.Headers;
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

    [Test]
    public async Task FlightSearch_RetriesOnce_WhenApiReturnsTooManyRequests()
    {
        var expectedFlights = _fixture.Create<FlightSearchOutputModel>();
        var httpResponseJson = JsonConvert.SerializeObject(expectedFlights);
        var rateLimitResponse = new HttpResponseMessage(HttpStatusCode.TooManyRequests);
        rateLimitResponse.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.Zero);
        var successfulResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(httpResponseJson)
        };
        var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        httpMessageHandlerMock.Protected()
            .SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(rateLimitResponse)
            .ReturnsAsync(successfulResponse);
        var httpClient = AmadeusApiHttpClientTestHelper.CreateTestHttpClient(httpMessageHandlerMock);
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        AmadeusApiHttpClientTestHelper.SetupHttpClientFactoryMock(httpClientFactoryMock, httpClient);
        var sut = new FlightSearchService(httpClientFactoryMock.Object, _getTokenMock.Object);

        var result = await sut.FlightSearch(_defaultParametersBuilder.Build());

        result.Should().BeEquivalentTo(expectedFlights);
        httpMessageHandlerMock.Protected().Verify(
            "SendAsync",
            Times.Exactly(2),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>());
    }
}
