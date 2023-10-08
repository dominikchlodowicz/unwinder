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
using unwinder.tests.Services.Helpers;

namespace unwinder.tests.Services.AmadeusApiService;

[TestFixture]
public class GetTokenTests
{
    private Mock<ILogger<GetToken>> _loggerMock;
    private string _serviceApiKey;
    private string _serviceApiSecretKey;

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<GetToken>>();

        _serviceApiKey = "testApiKey";
        _serviceApiSecretKey = "testApiSecretKey";
    }

    [Test]
    public async Task GetAuthToken_ReturnsToken_WhenEverythingIsRight()
    {
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        var httpMessageHandlerMock = new Mock<HttpMessageHandler>();

        var mockedReturnedJson = "{{\"access_token\": \"{expectedToken}\"}}";
        httpMessageHandlerMock = HttpClientTestHelper.SetupHttpMessageHandlerMock(HttpStatusCode.OK, mockedReturnedJson);
        var httpClient = HttpClientTestHelper.CreateTestHttpClient(httpMessageHandlerMock);
        HttpClientTestHelper.SetupHttpClientFactoryMock(httpClientFactoryMock, httpClient);

        var sut = new GetToken(httpClientFactoryMock.Object, _loggerMock.Object, _serviceApiKey, _serviceApiSecretKey);

        var token = await sut.GetAuthToken();

        Assert.That(token, Is.EqualTo(mockedReturnedJson));
    }

    [Test]
    [TestCase(HttpStatusCode.InternalServerError)]
    [TestCase(HttpStatusCode.NotFound)]
    [TestCase(HttpStatusCode.BadGateway)]
    public void GetAuthToken_ReturnsApiError_WhenApiResponseIsInvalid(HttpStatusCode statusCode)
    {
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        var httpMessageHandlerMock = new Mock<HttpMessageHandler>();

        httpMessageHandlerMock = HttpClientTestHelper.SetupHttpMessageHandlerMock(statusCode);

        var httpClient = HttpClientTestHelper.CreateTestHttpClient(httpMessageHandlerMock);

        HttpClientTestHelper.SetupHttpClientFactoryMock(httpClientFactoryMock, httpClient);

        var sut = new GetToken(httpClientFactoryMock.Object, _loggerMock.Object, _serviceApiKey, _serviceApiSecretKey);

        Assert.ThrowsAsync<HttpRequestException>(async () => await sut.GetAuthToken());
    }

    [TestCase("{}")]
    [TestCase(null)]
    public void GetAuthToken_ThrowsException_WhenResponseIsEmpty(string expectedToken)
    {
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        var httpMessageHandlerMock = new Mock<HttpMessageHandler>();

        var mockReturnedJson = expectedToken == null ? null : expectedToken;

        httpMessageHandlerMock = HttpClientTestHelper.SetupHttpMessageHandlerMock(HttpStatusCode.OK, mockReturnedJson);

        var httpClient = HttpClientTestHelper.CreateTestHttpClient(httpMessageHandlerMock);

        HttpClientTestHelper.SetupHttpClientFactoryMock(httpClientFactoryMock, httpClient);

        var sut = new GetToken(httpClientFactoryMock.Object, _loggerMock.Object, _serviceApiKey, _serviceApiSecretKey);

        Assert.ThrowsAsync<InvalidOperationException>(async () => await sut.GetAuthToken());
    }

    [Test]
    public void GetAuthToken_ThrowsException_WhenJsonStructureIsUnexpected()
    {
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        var httpMessageHandlerMock = new Mock<HttpMessageHandler>();

        var mockReturnedJson = "{\"unexpected_property\":\"unexpectedValue\", \"type\":\"sampleType\"}";

        httpMessageHandlerMock = HttpClientTestHelper.SetupHttpMessageHandlerMock(HttpStatusCode.OK, mockReturnedJson);

        var httpClient = HttpClientTestHelper.CreateTestHttpClient(httpMessageHandlerMock);

        HttpClientTestHelper.SetupHttpClientFactoryMock(httpClientFactoryMock, httpClient);

        var sut = new GetToken(httpClientFactoryMock.Object, _loggerMock.Object, _serviceApiKey, _serviceApiSecretKey);

        Assert.ThrowsAsync<JsonReaderException>(async () => await sut.GetAuthToken());
    }
}
