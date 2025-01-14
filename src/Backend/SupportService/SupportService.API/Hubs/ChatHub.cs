using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SupportService.Application.DTO;
using SupportService.Application.UseCases.CloseChat;
using SupportService.Application.UseCases.CreateChat;
using SupportService.Application.UseCases.GetAllChats;
using SupportService.Application.UseCases.GetChatMessages;
using SupportService.Application.UseCases.GetUserChats;
using SupportService.Application.UseCases.SendMessage;
using System.Security.Claims;

namespace SupportService.API.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IMediator _mediator;
    private readonly ILogger<ChatHub> _logger;

    public ChatHub(IMediator mediator, ILogger<ChatHub> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    
    public override async Task OnConnectedAsync()
    {
        var role = Context.User.FindFirst(ClaimTypes.Role)?.Value;
        var userId = Context.User.FindFirst("Id")?.Value;

        _logger.LogInformation($"User with id: {userId} connected to hub");

        if (role == "admin")
        {
            _logger.LogInformation($"User with id: {userId} added to group admin");

            await Groups.AddToGroupAsync(Context.ConnectionId, "admin");
        }
        else
        {
            _logger.LogInformation($"User with id: {userId} added to group {userId}");

            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }

        await base.OnConnectedAsync();
    }

    [Authorize(Policy = "admin")]
    public async Task GetAllChatsAsync()
    {
        var chats = await _mediator.Send(new GetAllChatsRequest());

        await Clients.Caller.SendAsync("ReceiveChats", chats);
    }

    public async Task GetUserChatsAsync()
    {
        var userId = Context.User.FindFirst("Id")?.Value;

        var chats = await _mediator.Send(new GetUserChatsRequest(userId));

        await Clients.Caller.SendAsync("ReceiveUserChats", chats);
    }

    public async Task GetChatMessagesAsync(int chatId)
    {
        var messages = await _mediator.Send(new GetChatMessagesRequest(chatId));

        await Clients.Caller.SendAsync("ReceiveChatMessages", messages);
    }

    public async Task CreateChatAsync()
    {
        var userId = Context.User.FindFirst("Id")?.Value;
        var userEmail = Context.User.FindFirst(ClaimTypes.Email)?.Value;

        var chat = await _mediator.Send(new CreateChatRequest(userId, userEmail));

        await Clients.Group("admin").SendAsync("ReceiveNewChat", chat);

        await Clients.Caller.SendAsync("ReceiveNewChat", chat);
    }

    public async Task CloseChatAsync(int chatId)
    {
        string? userId = null;
		var role = Context.User.FindFirst(ClaimTypes.Role)?.Value;

        if (role != "admin")
        {
            userId = Context.User.FindFirst("Id")?.Value;
        }

		var chat = await _mediator.Send(new CloseChatRequest(chatId, userId));

        await Clients.Group("admin").SendAsync("CloseChat", chat.Id);

        await Clients.Group(chat.ClientId).SendAsync("CloseChat", chat.Id);
    }

    public async Task SendMessageAsync(AddMessageDTO messageDto)
    {
        var userId = Context.User.FindFirst("Id")?.Value;

        var message = await _mediator.Send(new SendMessageRequest(messageDto, userId));

        await Clients.Group("admin").SendAsync("ReceiveMessage", message);

        await Clients.Group(message.ChatOwnerId).SendAsync("ReceiveMessage", message);
    }
}
