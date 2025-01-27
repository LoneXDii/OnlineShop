using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Moq;
using SupportService.API.Filters;
using SupportService.API.Hubs;
using SupportService.Tests.Factories;

namespace SupportService.Tests.UnitTests.Filters;

public class HubExceptionsFilterTests
{
    private readonly Mock<IHubContext<ChatHub>> _hubContextMock;
    private readonly Mock<ILogger<HubExceptionsFilter>> _loggerMock = new();
    private readonly HubExceptionsFilter _filter;

    public HubExceptionsFilterTests()
    {
        _hubContextMock = MocksFactory.CreateHubContext();
        
        _filter = new HubExceptionsFilter(_hubContextMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task InvokeMethodAsync_WhenNoExceptionOccurs_ShouldReturnResult()
    {
        //Arrange
        var invocationContext = MocksFactory.CreateHubInvocationContext("test-connection-id");
        
        Func<HubInvocationContext, ValueTask<object?>> next = _ => new ValueTask<object?>(42);

        //Act
        var result = await _filter.InvokeMethodAsync(invocationContext, next);

        //Assert
        Assert.Equal(42, result);
    }
    
    [Fact]
    public async Task InvokeMethodAsync_WhenExceptionOccurs_ShouldThrowHubException()
    {
        //Arrange
        var invocationContext = MocksFactory.CreateHubInvocationContext("test-connection-id");
        
        Func<HubInvocationContext, ValueTask<object?>> next = _ => throw new InvalidOperationException("Test exception");

        //Act
        var exception = await Assert.ThrowsAsync<HubException>(async () => 
            await _filter.InvokeMethodAsync(invocationContext, next));

        //Assert
        Assert.Equal("Test exception", exception.Message);
    }
}