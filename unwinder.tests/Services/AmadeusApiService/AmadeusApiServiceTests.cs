using System.Net;
using Moq.Protected;
using Microsoft.Extensions.Logging;
using unwinder.Services;
using unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;

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
                ItExpr.Is<HttpRequestMessage>(request => request.RequestUri.AbsolutePath.Contains("reference-data/locations")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"data\": [{\"name\": \"Airport Name\",\"iataCode\": \"ATC\","
                    + "\"address\": {\"cityName\": \"CityName\"}}]}")
            })
            .Verifiable();

        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(request => request.RequestUri.AbsolutePath.Contains("shopping/flight-offers")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"data\": [{\"flightSearchData\": \"Mocked flight search data\"}]}")
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
        var requestParameters = new FlightSearchParameters
        {
            CurrencyCode = "USD",
            OriginDestinations = new List<OriginDestination>
            {
                new OriginDestination
                {
                    Id = "1",
                    OriginLocationCode = "NYC",
                    DestinationLocationCode = "MAD",
                    DepartureDateTimeRange = new DepartureDateTimeRange
                    {
                        Date = "2023-11-01",
                        Time = "10:00:00"
                    }
                }
            },
            Travelers = new List<Traveler>
            {
                new Traveler
                {
                    Id = "1",
                    TravelerType = "ADULT"
                }
            },
            Sources = new List<string> { "GDS" },
            SearchCriteria = new SearchCriteria
            {
                MaxFlightOffers = 2,
                FlightFilters = new FlightFilters
                {
                    CabinRestrictions = new List<CabinRestriction>
                    {
                        new CabinRestriction
                        {
                            Cabin = "BUSINESS",
                            Coverage = "MOST_SEGMENTS",
                            OriginDestinationIds = new List<string> { "1" }
                        }
                    }
                }
            }
        };

        var result = await _service.FlightSearch(flightSearchParameters);

        Assert.IsNotNull(result);
    }
}
