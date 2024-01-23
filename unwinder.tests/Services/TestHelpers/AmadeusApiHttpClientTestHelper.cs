using System.Net;
using Moq.Protected;
using unwinder.Services.AmadeusApiService.FlightSearch;

namespace unwinder.tests.Services.Helpers;

public static class AmadeusApiHttpClientTestHelper
{

    public static IHttpClientFactory SetupHttpClient(HttpStatusCode statusCode, string mockReturnContent = "")
    {
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        var httpMessageHandlerMock = SetupHttpMessageHandlerMock(statusCode, mockReturnContent);
        var httpClient = CreateTestHttpClient(httpMessageHandlerMock);
        SetupHttpClientFactoryMock(httpClientFactoryMock, httpClient);

        return httpClientFactoryMock.Object;
    }

    public static Mock<HttpMessageHandler> SetupHttpMessageHandlerMock(HttpStatusCode statusCode, string content = "")
    {
        var httpMessageHandlerMock = new Mock<HttpMessageHandler>();

        httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = string.IsNullOrEmpty(content) ? null : new StringContent(content)
            });

        return httpMessageHandlerMock;
    }

    public static HttpClient CreateTestHttpClient(Mock<HttpMessageHandler> httpMessageHandler)
    {
        return new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri("http://test.com")
        };
    }

    public static void SetupHttpClientFactoryMock(Mock<IHttpClientFactory> httpClientFactoryMock, HttpClient httpClient)
    {
        httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
    }

    public static FlightSearchParametersBuilder CreateDefaultFlightSearchParametersBuilder()
    {
        string currentDate = DateTime.Now.Date.AddDays(1).ToString("yyyy-MM-dd");

        return new FlightSearchParametersBuilder()
            .BuildNumberOfTravelers(new List<string> { "ADULT" })
            .BuildDateTimeRange(currentDate, "10:30:00")
            .BuildOriginDestinations("WAW", "LAX")
            .BuildCurrencyCode("PLN")
            .BuildDefaultValues();
    }

}