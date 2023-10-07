using System.Net;
using Moq.Protected;
using Microsoft.Extensions.Logging;
using unwinder.Services;
using unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;
using unwinder.Services.AmadeusApiService;

namespace unwinder.tests.Services.AmadeusApiService;

public class GetLocationServiceTests
{
    private IAmadeusApiCommonService _commonService;

    private Mock<IGetToken> _getTokenMock;

    private Mock<IHttpClientFactory> _httpClientFactoryMock;
    private Mock<HttpMessageHandler> _httpMessageHandlerMock;

    private HttpClient _client;

    [SetUp]
    public void Setup()
    {
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();

        _getTokenMock = new Mock<IGetToken>();
        
        _getTokenMock.Setup(_ => _.GetAuthToken()).ReturnsAsync("test_token");

        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

        _httpMessageHandlerMock
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
        

        _client = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("http://test.com")
        };


        _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(_client);

        _commonService = new AmadeusApiCommonService(_httpClientFactoryMock.Object, _getTokenMock.Object);
    }

    [Test]
    public async Task GetLocationService_ReturnsLocation_WhenEverythingIsRight()
    {
        // Arrange
        var query = "test_query";
        var sut = new GetLocationService(_commonService);

        // Act
        var result = await sut.GetLocation(query);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count());
        var airport = result.First();
        Assert.AreEqual("Airport Name", airport.Name);
        Assert.AreEqual("ATC", airport.IataCode);
        Assert.AreEqual("CityName", airport.CityName);
        _httpMessageHandlerMock.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(request => 
                request.RequestUri.AbsolutePath.Contains("reference-data/locations") &&
                request.RequestUri.Query.Contains(query)),
            ItExpr.IsAny<CancellationToken>()
        );
    }

    [Test]
    [TestCase(HttpStatusCode.InternalServerError)]
    [TestCase(HttpStatusCode.NotFound)]
    [TestCase(HttpStatusCode.BadGateway)]
    public async Task GetLocationService_ReturnsApiError_WhenApiResponseIsInvalid(HttpStatusCode statusCode)
    {
        var query = "test_query";

        _httpMessageHandlerMock.Protected()
        .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
        .ReturnsAsync(new HttpResponseMessage
        {
            StatusCode = statusCode
        });

        var httpClient = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("http://test.com")
        };

        _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // System under test: "sut"
        var sut = new GetLocationService(_commonService);

        // What method should return(
        Assert.ThrowsAsync<HttpRequestException>(async () => await sut.GetLocation(query));
    }

    [Ignore("Todo")]
    [TestCase("{}")]
    [TestCase(null)]
    public async Task GetLocationService_ThrowsException_WhenResponseIsEmpty(string expectedToken)
    {

    }
 
    [Ignore("Todo")]
    [Test]
    public async Task GetLocationService_ThrowsException_WhenJsonStructureIsUnexpected()
    {

    }

}