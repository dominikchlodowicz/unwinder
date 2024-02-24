using System.Net;
using Newtonsoft.Json;
using unwidner.Models.AmadeusApiServiceModels.HotelSearchModels;
using unwinder.Services;
using unwinder.Services.AmadeusApiService.HotelSearch;
using unwinder.tests.Services.Helpers;

namespace unwinder.tests.Service.AmadeusApiService;

public class HotelSearchListServiceTests
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

    // [Test]
    // public void SearchListOfHotels_WithValidHotelSearchListParameters_ReturnsHotelSearchListOutputModel()
    // {
    //     var expectedHotels = _fixture.Create<HotelSearchListOutputModel>();
    //     var httpResponseJson = JsonConvert.SerializeObject(expectedHotels);
    //     var httpClientMock = AmadeusApiHttpClientTestHelper.SetupHttpClient(HttpStatusCode.OK, httpResponseJson);
    //     var hotelSearchListParametersModel = new HotelSearchListParametersModel
    //     {
    //         CityCode = "example"
    //     };

    //     var sut = new HotelSearchListService(httpClientMock, _getTokenMock.Object);

    //     var result = sut.SearchListOfHotels(hotelSearchListParametersModel);

    //     Assert.IsNotNull(result, "Result should not be null");
    //     Assert.IsInstanceOf<HotelSearchListOutputModel>(result, "Result should be of type HotelSearchOutputModel");
    // }

    [Test]
    public async Task SearchListOfHotels_WithValidHotelSearchListParameters_ReturnsHotelSearchListOutputModel()
    {
        var expectedHotels = _fixture.Create<HotelSearchListOutputModel>();
        var httpResponseJson = JsonConvert.SerializeObject(expectedHotels);

        var httpClientMock = AmadeusApiHttpClientTestHelper.SetupHttpClient(HttpStatusCode.OK, httpResponseJson);

        var sut = new HotelSearchListService(httpClientMock, _getTokenMock.Object);

        var hotelSearchListParametersModel = new HotelSearchListParametersModel
        {
            CityCode = "example"
        };

        var result = await sut.SearchListOfHotels(hotelSearchListParametersModel);

        Assert.IsNotNull(result, "Result should not be null");
        Assert.IsInstanceOf<HotelSearchListOutputModel>(result, "Result should be of type HotelSearchListOutputModel");
    }


    [Test]
    [TestCase(HttpStatusCode.InternalServerError)]
    [TestCase(HttpStatusCode.NotFound)]
    [TestCase(HttpStatusCode.BadGateway)]
    public void SearchListOfHotels_ThrowsHttpRequestException_WhenApiResponseIsInvalid(HttpStatusCode statusCode)
    {
        var expectedHotels = _fixture.Create<HotelSearchListOutputModel>();
        var httpResponseJson = JsonConvert.SerializeObject(expectedHotels);
        var httpClientMock = AmadeusApiHttpClientTestHelper.SetupHttpClient(statusCode, httpResponseJson);

        var sut = new HotelSearchListService(httpClientMock, _getTokenMock.Object);
        var sutParameters = new HotelSearchListParametersModel
        {
            CityCode = "example"
        };
        Assert.ThrowsAsync<HttpRequestException>(async () => await sut.SearchListOfHotels(sutParameters));
    }

}