using Grpc.Core;
using Microsoft.Extensions.Logging;
using Moq;
using ProductsService.API.Interceptors;

namespace ProductsService.Tests.UnitTests.Interceptors;

public class ExceptionsInterceptorTests
{
    private readonly Mock<ILogger<ExceptionsInterceptor>> _loggerMock = new();
    private readonly ExceptionsInterceptor _interceptor;

    public ExceptionsInterceptorTests()
    {
        _interceptor = new ExceptionsInterceptor(_loggerMock.Object);
    }

    [Fact]
    public async Task UnaryServerHandler_WhenNoExceptionThrown_ShouldProcessRequest()
    {
        //Arrange
        var request = new TestRequest { Message = "Test" };
        var response = new TestResponse { Result = "Success" };
        var context = new Mock<ServerCallContext>();
        var continuation = new UnaryServerMethod<TestRequest, TestResponse>((req, ctx) => Task.FromResult(response));

        //Act
        var result = await _interceptor.UnaryServerHandler(request, context.Object, continuation);

        //Assert
        Assert.Equal(response, result);
    }

    [Fact]
    public async Task UnaryServerHandler_WhenExceptionThrown_ShouldThrowRpcException()
    {
        //Arrange
        var request = new TestRequest { Message = "Test" };
        var context = new Mock<ServerCallContext>();
        var exceptionMessage = "Something went wrong";
        var continuation = new UnaryServerMethod<TestRequest, TestResponse>((req, ctx) => throw new InvalidOperationException(exceptionMessage));

        //Act
        var rpcException = await Assert.ThrowsAsync<RpcException>(() => _interceptor.UnaryServerHandler(request, context.Object, continuation));

        //Assert
        Assert.Equal(StatusCode.InvalidArgument, rpcException.StatusCode);
        Assert.Equal(exceptionMessage, rpcException.Status.Detail);
    }
}

public class TestRequest
{
    public string Message { get; set; }
}

public class TestResponse
{
    public string Result { get; set; }
}