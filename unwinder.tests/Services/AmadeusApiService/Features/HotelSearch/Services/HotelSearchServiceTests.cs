using System.Net;
using Moq.Protected;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Newtonsoft.Json;
using unwidner.Models.AmadeusApiServiceModels.HotelSearchModels;
using unwinder.Services;
using unwinder.Services.AmadeusApiService.HotelSearch;
using unwinder.Services.HelperServices;
using unwinder.tests.Services.Helpers;

namespace unwinder.tests.Service.AmadeusApiService;

public class HotelSearchServiceTests
{
    private Mock<IGetToken> _getTokenMock;

    private Fixture _fixture;

    private Mock<ICurrencyConversionService> _currencyConversionServiceMock;

    [SetUp]
    public void Setup()
    {
        _getTokenMock = new Mock<IGetToken>();
        _getTokenMock.Setup(_ => _.GetAuthToken()).ReturnsAsync("test_token");
        _fixture = new Fixture();
        _currencyConversionServiceMock = new Mock<ICurrencyConversionService>();
    }

    [Test]
    public async Task SearchHotel_WithValidHotelSearchParameters_ReturnsHotelSearchOutputModel()
    {
        var expectedHotels = _fixture.Create<HotelSearchOutputModel>();
        var httpResponseJson = JsonConvert.SerializeObject(expectedHotels);
        var httpClientMock = AmadeusApiHttpClientTestHelper.SetupHttpClient(HttpStatusCode.OK, httpResponseJson);
        var hotelSearchParameters = new HotelSearchParametersModel
        {
            HotelIds = new List<string> { "1", "2", "3" }
        };

        var sut = new HotelSearchService(httpClientMock, _getTokenMock.Object, _currencyConversionServiceMock.Object);

        var result = await sut.SearchHotel(hotelSearchParameters);

        Assert.IsNotNull(result, "Result should not be null");
        Assert.IsInstanceOf<HotelSearchOutputModel>(result, "Result should be of type HotelSearchOutputModel");
    }

    [Test]
    public async Task SearchHotel_SendsAdultsParameterInRequestUrl()
    {
        var expectedHotels = _fixture.Create<HotelSearchOutputModel>();
        var httpResponseJson = JsonConvert.SerializeObject(expectedHotels);
        Uri requestedUri = null;
        var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Callback<HttpRequestMessage, CancellationToken>((request, _) => requestedUri = request.RequestUri)
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(httpResponseJson)
            });
        var httpClient = AmadeusApiHttpClientTestHelper.CreateTestHttpClient(httpMessageHandlerMock);
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        AmadeusApiHttpClientTestHelper.SetupHttpClientFactoryMock(httpClientFactoryMock, httpClient);
        var hotelSearchParameters = new HotelSearchParametersModel
        {
            HotelIds = new List<string> { "1", "2", "3" },
            Adults = 2,
            CheckInDate = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd"),
            CheckOutDate = DateTime.Today.AddDays(2).ToString("yyyy-MM-dd"),
            Currency = "EUR"
        };
        var sut = new HotelSearchService(
            httpClientFactoryMock.Object,
            _getTokenMock.Object,
            _currencyConversionServiceMock.Object);

        await sut.SearchHotel(hotelSearchParameters);

        Assert.That(requestedUri, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(requestedUri.Query, Does.Contain("adults=2"));
            Assert.That(requestedUri.Query, Does.Not.Contain("aduts="));
        });
    }

    [Test]
    [TestCase(HttpStatusCode.InternalServerError)]
    [TestCase(HttpStatusCode.NotFound)]
    [TestCase(HttpStatusCode.BadGateway)]
    public void SearchHotel_ThrowsHttpRequestException_WhenApiResponseIsInvalid(HttpStatusCode statusCode)
    {
        var expectedHotels = _fixture.Create<HotelSearchListOutputModel>();
        var httpResponseJson = JsonConvert.SerializeObject(expectedHotels);
        var httpClientMock = AmadeusApiHttpClientTestHelper.SetupHttpClient(statusCode, httpResponseJson);

        var sutParameters = new HotelSearchParametersModel
        {
            HotelIds = new List<string> { "1", "2", "3" }
        };

        var sut = new HotelSearchService(httpClientMock, _getTokenMock.Object, _currencyConversionServiceMock.Object);

        Assert.ThrowsAsync<HttpRequestException>(async () => await sut.SearchHotel(sutParameters));
    }
}
