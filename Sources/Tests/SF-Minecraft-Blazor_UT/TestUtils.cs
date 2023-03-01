using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json.Linq;

namespace SF_Minecraft_Blazor_UT;

public static class TestUtils
{
    private const string BaseUrl = "https://localhost:7234/api/";

    public static HttpClient GetHttpClientRead(object apiController, string methodName, string endpoint, params object[]? parameters)
    {
        object? result = null;
        try
        {
            result = apiController.GetType().GetMethod(methodName)!.Invoke(apiController, parameters);
        }
        catch (Exception)
        {
            // ignored
        }

        var jsonResult = result is not null ? JObject.Parse(JsonSerializer.Serialize(result))["Result"]!.ToString() : "";


            var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
                   .Setup<Task<HttpResponseMessage>>(
                       "SendAsync",
                       ItExpr.Is<HttpRequestMessage>(
                           req => req.RequestUri!.ToString() == BaseUrl + endpoint
                       ),
                       ItExpr.IsAny<CancellationToken>()
                   )
                   .ReturnsAsync(new HttpResponseMessage
                       {
                           StatusCode = HttpStatusCode.OK,
                           Content = new StringContent(
                               jsonResult, 
                               Encoding.UTF8, 
                               "application/json"
                           )
                       }
                   );

        return new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri(BaseUrl)
        };
    }

    public static HttpClient GetHttpClientWrite(object apiController, string methodName, string endpoint, Type requestType, params object[] parameters)
    {
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
                   .Setup<Task<HttpResponseMessage>>(
                       "SendAsync",
                       ItExpr.Is<HttpRequestMessage>(
                           req => req.RequestUri!.ToString() == BaseUrl + endpoint
                       ),
                       ItExpr.IsAny<CancellationToken>()
                   )
                   .ReturnsAsync(CreateResponseMessage(apiController, methodName, parameters)
                   );

        return new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri(BaseUrl)
        };
    }
    private static HttpResponseMessage CreateResponseMessage(object controller, string methodName, params object[] parameters)
    {
        try
        {
            controller.GetType().GetMethod(methodName)!.Invoke(controller, parameters);
        }
        catch (Exception)
        {
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = null
            };
        }
        
        return new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("", Encoding.UTF8, "application/json")
        };
    }
    
    public static ILogger<T> CreateLogger<T>() 
        => new Mock<ILogger<T>>().Object;
}
