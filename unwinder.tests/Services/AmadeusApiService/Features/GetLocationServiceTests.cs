using System.Net;
using Moq.Protected;
using unwinder.Services;
using unwinder.Services.AmadeusApiService;
using unwinder.tests.Services.Helpers;
using unwinder.Models.AmadeusApiServiceModels.GetLocationModels;
using Newtonsoft.Json;

namespace unwinder.tests.Services.AmadeusApiService;

public class GetLocationServiceTests
{
    private Mock<IGetToken> _getTokenMock;
    private Mock<IHttpClientFactory> _httpClientFactoryMock;
    private Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private HttpClient _client;

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
        var expectedLocations = _fixture.Create<GetLocationAirportModel>();
        var wrapper = new GetLocationAirportModelWrapper { data = expectedLocations };
        var httpResponseJson = JsonConvert.SerializeObject(wrapper);

        var httpCleintMock = HttpClientTestHelper.SetupHttpClient(HttpStatusCode.OK, httpResponseJson);
        var sut = new GetLocationService(httpCleintMock, _getTokenMock.Object);

        // Act
        var result = await sut.GetLocation("test");

        Assert.That(expectedLocations, Is.EqualTo(result));
    }

    [Test]
    [TestCase(HttpStatusCode.InternalServerError)]
    [TestCase(HttpStatusCode.NotFound)]
    [TestCase(HttpStatusCode.BadGateway)]
    public async Task GetLocationService_ReturnsApiError_WhenApiResponseIsInvalid(HttpStatusCode statusCode)
    {
        var httpCleintMock = HttpClientTestHelper.SetupHttpClient(statusCode);
        var sut = new GetLocationService(httpCleintMock, _getTokenMock.Object);

        Assert.ThrowsAsync<HttpRequestException>(async () => await sut.GetLocation("test"));
    }

    // [TestCase("{}")]
    // [TestCase(null)]
    // public void GetLocationService_ThrowsException_WhenResponseIsEmpty(string expectedToken)
    // {
    //     var httpClientFactoryMock = new Mock<IHttpClientFactory>();
    //     var httpMessageHandlerMock = new Mock<HttpMessageHandler>();

    //     var query = "test_query";

    //     _httpMessageHandlerMock.Protected()
    //     .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
    //     .ReturnsAsync(new HttpResponseMessage
    //     {
    //         StatusCode = HttpStatusCode.OK,
    //         Content = expectedToken == null ? null : new StringContent(expectedToken)

    //     });

    //     var httpClient = new HttpClient(_httpMessageHandlerMock.Object)
    //     {
    //         BaseAddress = new Uri("http://test.com")
    //     };

    //     _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

    //     // System under test: "sut"
    //     var sut = new GetLocationService(_commonService);

    //     // What method should return(
    //     Assert.ThrowsAsync<InvalidOperationException>(async () => await sut.GetLocation(query));
    // }
 
    // [Test]
    // public void GetLocationService_ThrowsException_WhenJsonStructureIsUnexpected()
    // {

    //     var httpClientFactoryMock = new Mock<IHttpClientFactory>();
    //     var httpMessageHandlerMock = new Mock<HttpMessageHandler>();

    //     var query = "test_query";
        
    //     var mockResponseContent = "{\"someOtherKey\": \"someValue\", \"anotherKey\": [1, 2, 3]}";

    //     _httpMessageHandlerMock.Protected()
    //     .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
    //     .ReturnsAsync(new HttpResponseMessage
    //     {
    //         StatusCode = HttpStatusCode.OK,
    //         Content = new StringContent(mockResponseContent)

    //     });

    //     var httpClient = new HttpClient(_httpMessageHandlerMock.Object)
    //     {
    //         BaseAddress = new Uri("http://test.com")
    //     };

    //     _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

    //     // System under test: "sut"
    //     var sut = new GetLocationService(_commonService);

    //     // What method should return(
    //     Assert.ThrowsAsync<InvalidOperationException>(async () => await sut.GetLocation(query));
    // }
}


