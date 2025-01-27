using System.Security.Claims;
using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Moq;
using SupportService.API.Hubs;
using SupportService.Application.DTO;
using SupportService.Application.UseCases.CloseChat;
using SupportService.Application.UseCases.CreateChat;
using SupportService.Application.UseCases.GetAllChats;
using SupportService.Application.UseCases.GetChatById;
using SupportService.Application.UseCases.GetChatMessages;
using SupportService.Application.UseCases.GetUserChats;
using SupportService.Application.UseCases.SendMessage;

namespace SupportService.Tests.UnitTests.Hubs;

public class ChatHubTests
{
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly Mock<ILogger<ChatHub>> _loggerMock = new();
    private readonly Mock<IClientProxy> _clientProxyMock = new();
    private readonly Mock<ISingleClientProxy> _singleClientProxyMock = new();
    private readonly Mock<HubCallerContext> _hubCallerContextMock = new();
    private readonly Mock<IGroupManager> _groupManagerMock = new();
    private readonly ChatHub _hub;
    private readonly Fixture _fixture = new();
    
    public ChatHubTests()
    {
        _hub = new ChatHub(_mediatorMock.Object, _loggerMock.Object);
        
        _hubCallerContextMock.Setup(context => context.ConnectionId).Returns("connectionId");
        
        var hubCallerClientsMock = new Mock<IHubCallerClients>();
        hubCallerClientsMock.Setup(clients => clients.Caller).Returns(_singleClientProxyMock.Object);
        hubCallerClientsMock.Setup(clients => clients.Group(It.IsAny<string>())).Returns(_clientProxyMock.Object);
        
        _hub.Context = _hubCallerContextMock.Object;
        _hub.Clients = hubCallerClientsMock.Object;
        _hub.Groups = _groupManagerMock.Object;
        
        var userId = Guid.NewGuid().ToString();
        var userEmail = "user@example.com";
        _hubCallerContextMock.Setup(context => context.User).Returns(new ClaimsPrincipal(new ClaimsIdentity([
            new Claim("Id", userId),
            new Claim(ClaimTypes.Email, userEmail)
        ])));
    }

    [Fact]
    public async Task OnConnectedAsync_WhenUserIsAdmin_ShouldAddToAdminsGroup()
    {
        //Arrange
        var userId = Guid.NewGuid().ToString();
        _hubCallerContextMock.Setup(context => context.User).Returns(new ClaimsPrincipal(new ClaimsIdentity([
            new Claim("Id", userId),
            new Claim(ClaimTypes.Role, "admin")
        ])));
        
        //Act
        await _hub.OnConnectedAsync();
        
        //Assert
        _groupManagerMock.Verify(group => group.AddToGroupAsync(_hub.Context.ConnectionId, 
            "admin", CancellationToken.None), Times.Once);
    }
    
    [Fact]
    public async Task OnConnectedAsync_WhenUserIsNotAdmin_ShouldAddThisUserGroup()
    {
        //Arrange
        var userId = Guid.NewGuid().ToString();
        _hubCallerContextMock.Setup(context => context.User).Returns(new ClaimsPrincipal(new ClaimsIdentity([
            new Claim("Id", userId)
        ])));
        
        //Act
        await _hub.OnConnectedAsync();
        
        //Assert
        _groupManagerMock.Verify(group => group.AddToGroupAsync(_hub.Context.ConnectionId, 
            userId, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetAllChatsAsync_WhenCalled_ShouldSendGetAllChatsRequest()
    {
        //Arrange
        var chats = new List<ChatDTO>();
        
        _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<GetAllChatsRequest>(), 
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(chats);
        
        //Act
        await _hub.GetAllChatsAsync();
        
        //Assert
        _mediatorMock.Verify(mediator => mediator.Send(It.IsAny<GetAllChatsRequest>(), 
            It.IsAny<CancellationToken>()), Times.Once);
    }  
    [Fact]
    public async Task GetUserChatsAsync_WhenCalled_ShouldSendGetUserChatsRequest()
    {
        //Arrange
        var userChats = new List<ChatDTO>();
        _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<GetUserChatsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userChats);

        //Act
        await _hub.GetUserChatsAsync();

        //Assert
        _mediatorMock.Verify(mediator => mediator.Send(It.IsAny<GetUserChatsRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetChatMessagesAsync_WhenCalled_ShouldSendGetChatMessagesRequest()
    {
        //Arrange
        var chatId = 1;
        var messages = new List<MessageDTO>();
        _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<GetChatMessagesRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(messages);

        //Act
        await _hub.GetChatMessagesAsync(chatId);

        //Assert
        _mediatorMock.Verify(mediator => mediator.Send(It.IsAny<GetChatMessagesRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateChatAsync_WhenCalled_ShouldSendCreateChatRequest()
    {
        //Arrange
        var chat = _fixture.Create<ChatDTO>();
        _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<CreateChatRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(chat);

        //Act
        await _hub.CreateChatAsync();

        //Assert
        _mediatorMock.Verify(mediator => mediator.Send(It.IsAny<CreateChatRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetChatByIdAsync_WhenCalled_ShouldSendGetChatByIdRequest()
    {
        //Arrange
        var chatId = 1;
        var chat = _fixture.Create<ChatDTO>();
        _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<GetChatByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(chat);

        //Act
        await _hub.GetChatByIdAsync(chatId);

        //Assert
        _mediatorMock.Verify(mediator => mediator.Send(It.IsAny<GetChatByIdRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CloseChatAsync_WhenCalled_ShouldSendCloseChatRequest()
    {
        //Arrange
        var chatId = 1;
        var chat = _fixture.Create<ChatDTO>();
        _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<CloseChatRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(chat);

        //Act
        await _hub.CloseChatAsync(chatId);

        //Assert
        _mediatorMock.Verify(mediator => mediator.Send(It.IsAny<CloseChatRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SendMessageAsync_WhenCalled_ShouldSendMessageRequest()
    {
        // Arrange
        var messageDto = _fixture.Create<AddMessageDTO>();
        var message = _fixture.Create<MessageDTO>();
        _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<SendMessageRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(message);

        //Act
        await _hub.SendMessageAsync(messageDto);

        //Assert
        _mediatorMock.Verify(mediator => mediator.Send(It.IsAny<SendMessageRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}