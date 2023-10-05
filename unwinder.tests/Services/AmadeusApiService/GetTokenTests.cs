using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using Microsoft.Extensions.Logging;
using unwinder.Services;
using Newtonsoft.Json;

namespace unwinder.tests.Services.AmadeusApiService;

[TestFixture]
public class GetTokenTests
{
    private Mock<IHttpClientFactory> _httpClientFactoryMock;
    private Mock<ILogger<GetToken>> _loggerMock;
    private Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private string _serviceApiKey;
    private string _serviceApiSecretKey;

    [SetUp]
    public void Setup()
    {
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _loggerMock = new Mock<ILogger<GetToken>>();
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

        _serviceApiKey = "testApiKey";
        _serviceApiSecretKey = "testApiSecretKey";
    }

    [Test]
    public async Task GetAuthToken_ReturnsToken_WhenEverythingIsRight()
    {
        var expectedToken = "expectedToken";

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent($"{{\"access_token\": \"{expectedToken}\"}}")
            });

        var httpClient = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("http://test.com")
        };
    
        _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // System under test: "sut"
        var sut = new GetToken(_httpClientFactoryMock.Object, _loggerMock.Object, _serviceApiKey, _serviceApiSecretKey);

        var token = await sut.GetAuthToken();

        Assert.That(token, Is.EqualTo(expectedToken));
    }

    [Test]
    [TestCase(HttpStatusCode.InternalServerError)]
    [TestCase(HttpStatusCode.NotFound)]
    [TestCase(HttpStatusCode.BadGateway)]
    public async Task GetAuthToken_ReturnsApiError_WhenApiResponseIsInvalid(HttpStatusCode statusCode)
    {
        // mocking dependencies
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
        var sut = new GetToken(_httpClientFactoryMock.Object, _loggerMock.Object, _serviceApiKey, _serviceApiSecretKey);

        // What method should return(
        Assert.ThrowsAsync<HttpRequestException>(async () => await sut.GetAuthToken());
    }

    [TestCase("{}")]
    [TestCase(null)]
    public async Task GetAuthToken_ThrowsException_WhenResponseIsEmpty(string expectedToken)
    {   
        // mocking dependencies
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = expectedToken == null ? null : new StringContent(expectedToken)
            });

        var httpClient = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("http://test.com")
        };
    
        _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // System under test: "sut"
        var sut = new GetToken(_httpClientFactoryMock.Object, _loggerMock.Object, _serviceApiKey, _serviceApiSecretKey);

        // What method should return(
        Assert.ThrowsAsync<JsonSerializationException>(async () => await sut.GetAuthToken());
    }

    [Test]
    public async Task GetAuthToken_ThrowsException_WhenJsonStructureIsUnexpected()
    {
        var unexpectedJson = "{\"unexpected_property\":\"unexpectedValue\", \"type\":\"sampleType\"}"; 

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent($"{{\"access_token\": \"{unexpectedJson}\"}}")
            });

        var httpClient = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("http://test.com")
        };
    
        _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // System under test: "sut"
        var sut = new GetToken(_httpClientFactoryMock.Object, _loggerMock.Object, _serviceApiKey, _serviceApiSecretKey);

        Assert.ThrowsAsync<JsonReaderException>(async () => await sut.GetAuthToken());
    }
}
