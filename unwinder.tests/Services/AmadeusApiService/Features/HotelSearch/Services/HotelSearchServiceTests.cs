using System.Net;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Newtonsoft.Json;
using unwidner.Models.AmadeusApiServiceModels.HotelSearchModels;
using unwinder.Services;
using unwinder.Services.AmadeusApiService.HotelSearch;
using unwinder.tests.Services.Helpers;

namespace unwinder.tests.Service.AmadeusApiService;

public class HotelSearchServiceTests
{
    private Mock<IGetToken> _getTokenMock;

    private Fixture _fixture;

    [SetUp]
    public void Setup()
    {
        _getTokenMock = new Mock<IGetToken>();
        _getTokenMock.Setup(_ => _.GetAuthToken()).ReturnsAsync("test_token");
        _fixture = new Fixture();

    }

    [Test]
    public async Task SearchHotel_WithValidHotelSearchParameters_ReturnsHotelSearchOutputModel()
    {
        var expectedHotels = _fixture.Create<HotelSearchOutputModel>();
        var httpResponseJson = JsonConvert.SerializeObject(expectedHotels);
        var httpClientMock = AmadeusApiHttpClientTestHelper.SetupHttpClient(HttpStatusCode.OK, httpResponseJson);
        var hotelSearchParameters = new HotelSearchParametersModel();

        var sut = new HotelSearchService(httpClientMock, _getTokenMock.Object);

        var result = await sut.SearchHotel(hotelSearchParameters);

        Assert.IsNotNull(result, "Result should not be null");
        Assert.IsInstanceOf<HotelSearchOutputModel>(result, "Result should be of type HotelSearchOutputModel");
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

        var sut = new HotelSearchService(httpClientMock, _getTokenMock.Object);

        var sutParameters = new HotelSearchParametersModel();
        Assert.ThrowsAsync<HttpRequestException>(async () => await sut.SearchHotel(sutParameters));
    }
}