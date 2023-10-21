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
using unwinder.Models.AmadeusApiServiceModels.KeyModels;

namespace unwinder.tests.Services.AmadeusApiService;

[TestFixture]
public class GetTokenTests
{
    private Mock<ILogger<GetToken>> _loggerMock;
    private string _serviceApiKey;
    private string _serviceApiSecretKey;
    private Fixture _fixture;

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<GetToken>>();

        _serviceApiKey = "testApiKey";
        _serviceApiSecretKey = "testApiSecretKey";
        _fixture = new Fixture();
    }

    [Test]
    public async Task GetAuthToken_ReturnsToken_WhenEverythingIsRight()
    {
        var mockedReturnedJsonFixture = _fixture.Create<BearerTokenModel>();

        var mockedReturnedJsonSerialized = JsonConvert.SerializeObject(mockedReturnedJsonFixture);

        var httpClientMock = HttpClientTestHelper.SetupHttpClient(HttpStatusCode.OK, mockedReturnedJsonSerialized);

        var sut = new GetToken(httpClientMock, _loggerMock.Object, _serviceApiKey, _serviceApiSecretKey);

        var token = await sut.GetAuthToken();

        Assert.That(token, Is.EqualTo(mockedReturnedJsonFixture.access_token));
    }

    [Test]
    [TestCase(HttpStatusCode.InternalServerError)]
    [TestCase(HttpStatusCode.NotFound)]
    [TestCase(HttpStatusCode.BadGateway)]
    public void GetAuthToken_ReturnsApiError_WhenApiResponseIsInvalid(HttpStatusCode statusCode)
    {
        var httpClientMock = HttpClientTestHelper.SetupHttpClient(statusCode);

        var sut = new GetToken(httpClientMock, _loggerMock.Object, _serviceApiKey, _serviceApiSecretKey);

        Assert.ThrowsAsync<HttpRequestException>(async () => await sut.GetAuthToken());
    }

    [TestCase("{}")]
    [TestCase(null)]
    public void GetAuthToken_ThrowsException_WhenResponseIsEmpty(string expectedToken)
    {
        var mockReturnedJson = expectedToken == null ? null : expectedToken;

        var httpClientMock = HttpClientTestHelper.SetupHttpClient(HttpStatusCode.OK, mockReturnedJson);

        var sut = new GetToken(httpClientMock, _loggerMock.Object, _serviceApiKey, _serviceApiSecretKey);

        Assert.ThrowsAsync<InvalidOperationException>(async () => await sut.GetAuthToken());
    }

    [Test]
    public void GetAuthToken_ThrowsException_WhenJsonStructureIsUnexpected()
    {
        var mockReturnedJsonFull = _fixture.Create<BearerTokenModel>();
        mockReturnedJsonFull.access_token = null;
        var mockedReturnedJsonSerialized = JsonConvert.SerializeObject(mockReturnedJsonFull);

        var httpClientMock = HttpClientTestHelper.SetupHttpClient(HttpStatusCode.OK, mockedReturnedJsonSerialized);

        var sut = new GetToken(httpClientMock, _loggerMock.Object, _serviceApiKey, _serviceApiSecretKey);

        Assert.ThrowsAsync<JsonSerializationException>(async () => await sut.GetAuthToken());
    }
}
