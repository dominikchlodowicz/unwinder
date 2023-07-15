using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using Microsoft.Extensions.Logging;
using unwinder.Services;

namespace unwinder.tests.Services;

[TestFixture]
public class AmadeusApiServiceTests
{
    // private HttpClient _httpClient;
    private Mock<ILogger<IAmadeusApiService>> _loggerMock;
    private Mock<IGetToken> _bearerTokenMock;

    private Mock<IHttpClientFactory> _httpClientFactoryMock;
    private Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private AmadeusApiService _service;


    private HttpClient _client;

    [SetUp]
    public void Setup()
    {
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _loggerMock = new Mock<ILogger<IAmadeusApiService>>();
        _bearerTokenMock = new Mock<IGetToken>();
        

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"data\": [{\"name\": \"Airport Name\",\"iataCode\": \"ATC\"," 
                    + "\"address\": {\"cityName\": \"CityName\"}}]}")
            })
            .Verifiable();

            _client = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://test.com")
            };

            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(_client);

            _service = new AmadeusApiService(_httpClientFactoryMock.Object, _loggerMock.Object, _bearerTokenMock.Object);
    }

    [Test]
    public async Task GetLocation_ShouldReturnLocation()
    {
        // Arrange
        var service = new AmadeusApiService(_httpClientFactoryMock.Object, _loggerMock.Object, _bearerTokenMock.Object);
        _bearerTokenMock.Setup(_ => _.GetAuthToken()).ReturnsAsync("token");

        // Act
        var result = await service.GetLocation("query");

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("Airport Name"));
        Assert.IsTrue(result.Contains("ATC"));
        Assert.IsTrue(result.Contains("CityName"));
    }


    [Test]
    public async Task FlightSearch_ReturnsExpectedResult()
    {
        var flightSearchParameters = new FlightSearchParameters
        {
            OriginLocationCode = "LAX",
            DestinationLocationCode = "JFK",
            DepartureDate = DateTime.Now.AddDays(14),
            Adults = 1,
            Max = 10
        };

        var result = await _service.FlightSearch(flightSearchParameters);

        Assert.IsNotNull(result);
        //TODO: Additional assertions based on the expected result...
    }
}
