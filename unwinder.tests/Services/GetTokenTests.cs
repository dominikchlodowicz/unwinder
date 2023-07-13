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
    public async Task GetAuthToken_ShouldReturnToken()
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
}
