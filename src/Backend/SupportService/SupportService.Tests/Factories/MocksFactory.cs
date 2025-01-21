using System.Reflection;
using Microsoft.AspNetCore.SignalR;
using Moq;
using SupportService.API.Hubs;

namespace SupportService.Tests.Factories;

public static class MocksFactory
{
    public static Mock<IHubContext<ChatHub>> CreateHubContext()
    {
        var mock = new Mock<IHubContext<ChatHub>>();
        
        var hubClientsMock = new Mock<IHubClients>();
        var singleClientProxyMock = new Mock<ISingleClientProxy>();
        hubClientsMock.Setup(clients => clients.Client(It.IsAny<string>()))
            .Returns(singleClientProxyMock.Object);
        
        mock.Setup(h => h.Clients).Returns(hubClientsMock.Object);
        
        return mock;
    }
    
    public static HubInvocationContext CreateHubInvocationContext(string connectionId)
    {
        var callerContextMock = new Mock<HubCallerContext>();
        var serviceProviderMock = new Mock<IServiceProvider>();
        var hubMock = new Mock<Hub>();
        var methodInfoMock = new Mock<MethodInfo>();
        
        callerContextMock.Setup(x => x.ConnectionId).Returns(connectionId);
        
        return new HubInvocationContext(
            callerContextMock.Object,
            serviceProviderMock.Object,
            hubMock.Object,
            methodInfoMock.Object,
            []
        );
    }
}