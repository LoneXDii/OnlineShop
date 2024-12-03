using FluentValidation;
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
    private readonly IValidator<int> _chatIdValidator;
    private readonly IValidator<AddMessageDTO> _addMessageDTOValidator;

    public ChatHub(IMediator mediator, IValidator<int> chatIdValidator, IValidator<AddMessageDTO> addMessageDTOValidator)
    {
        _mediator = mediator;
        _chatIdValidator = chatIdValidator;
        _addMessageDTOValidator = addMessageDTOValidator;
    }
    
    public override async Task OnConnectedAsync()
    {
        var role = Context.User.FindFirst(ClaimTypes.Role)?.Value;
        var userId = Context.User.FindFirst("Id")?.Value;

        if (role == "admin")
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "admin");
        }
        else
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }

        await base.OnConnectedAsync();
    }

    [Authorize(Policy = "admin")]
    public async Task GetAllChatsAsync()
    {
        var chats = await _mediator.Send(new GetAllChatsRequest());

        await Clients.Caller.SendAsync("RecieveChats", chats);
    }

    public async Task GetUserChastAsync()
    {
        var userId = Context.User.FindFirst("Id")?.Value;

        var chats = await _mediator.Send(new GetUserChatsRequest(userId));

        await Clients.Caller.SendAsync("RecieveUserChats", chats);
    }

    public async Task GetChatMessages(int chatId)
    {
        var validationResult = _chatIdValidator.Validate(chatId);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var messages = await _mediator.Send(new GetChatMessagesRequest(chatId));

        await Clients.Caller.SendAsync("RecieveChatMessages", messages);
    }

    public async Task CreateChatAsync()
    {
        var userId = Context.User.FindFirst("Id")?.Value;
        var userEmail = Context.User.FindFirst(ClaimTypes.Email)?.Value;

        var chat = await _mediator.Send(new CreateChatRequest(userId, userEmail));

        await Clients.Group("admin").SendAsync("RecieveNewChat", chat);

        await Clients.Caller.SendAsync("RecieveNewChat", chat);
    }

    //Add admin check here
    public async Task CloseChatAsync(int chatId)
    {
        var validationResult = _chatIdValidator.Validate(chatId);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        string? userId = null;
		var role = Context.User.FindFirst(ClaimTypes.Role)?.Value;

        if (role != "admin")
        {
            userId = Context.User.FindFirst("Id")?.Value;
        }

		var chat = await _mediator.Send(new CloseChatRequest(chatId, userId));

        await Clients.Group("admin").SendAsync("RecieveNewChat", chat);

        await Clients.Group(chat.ClientId).SendAsync("RecieveNewChat", chat);
    }

    public async Task SendMessageAsync(AddMessageDTO messageDto)
    {
        var validationResult = _addMessageDTOValidator.Validate(messageDto);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var userId = Context.User.FindFirst("Id")?.Value;

        var message = await _mediator.Send(new SendMessageRequest(messageDto, userId));

        await Clients.Group("admin").SendAsync("RecieveMessage", message);

        await Clients.Group(message.ChatOwnerId).SendAsync("RecieveMessage", message);
    }
}
