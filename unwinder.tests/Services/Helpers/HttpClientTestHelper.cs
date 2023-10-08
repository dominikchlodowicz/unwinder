using System.Net;
using Moq.Protected;

namespace unwinder.tests.Services.Helpers;

public static class HttpClientTestHelper
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
                Content = new StringContent(content)
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
}